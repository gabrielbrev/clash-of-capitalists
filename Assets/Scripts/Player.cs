using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    public Renderer modelRenderer;
    [SerializeField] private GameObject panelPrefab;
    [SerializeField] private float moveSpeed;
    [SerializeField] protected int balance;
    private int index;
    private int prisonTime;
    private Tile currTile;
    private readonly List<PropertyTile> properties = new();
    private CapsuleCollider playerCollider;
    private Vector3 targetPos;
    protected PlayerPanel panel;

    private void UpdatePosition()
    {
        if (!currTile) return;
        targetPos = currTile.GetPlayerPosition(this);
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    private void UpdatePanel()
    {
        panel.SetInfo(
            $"{name}\n{balance:C}\nPosição {index}\n{(prisonTime > 0 ? $"Tempo de prisão: {prisonTime}" : "")}"
        );
    }

    protected void HandleDiceResult((int, int) result, System.Action<(int result, bool equalValues)> callback)
    {
        StartCoroutine(panel.SetDiceResult(result));

        if (result.Item1 == result.Item2) prisonTime = 0;

        callback.Invoke((
            prisonTime > 0 ? 0 : result.Item1 + result.Item2,
            result.Item1 == result.Item2
        ));

        if (prisonTime > 0) prisonTime--;
    }

    public void AddProperty(PropertyTile property)
    {
        properties.Add(property);
    }

    public int GetIndex()
    {
        return index;
    }

    public CapsuleCollider GetCollider()
    {
        return playerCollider;
    }

    public void MoveTo(Tile tile)
    {
        if (currTile) currTile.RemovePlayer(this);
        tile.AddPlayer(this);

        currTile = tile;
        index = tile.GetIndex();
    }

    public void AddBalance(int amount)
    {
        balance += amount;
        panel.SetBalanceText(amount);
    }

    public IEnumerator SubtractBalance(int amount)
    {
        if (balance < amount)
        {
            // TODO: Adicionar regra para vender propriedade ou falir ao tentar subtrair um valor maior do que o saldo
        }
        
        balance -= amount;
        panel.SetBalanceText(-amount);
        yield break;
    }

    public int GetNetWorth()
    {
        int propertyPrices = 0;

        foreach (PropertyTile property in properties)
        {
            propertyPrices += property.GetSellPrice();
        }

        return balance + propertyPrices;
    }

    public PlayerPanel GetPanel()
    {
        return panel;
    }

    public int GetPrisonRounds()
    {
        return prisonTime;
    }

    public void SetPrisonRounds(int num)
    {
        prisonTime = num;
    }

    public Color GetColor()
    {
        return modelRenderer.material.color;
    }

    public virtual IEnumerator OptRollDice(System.Action<(int result, bool equalValues)> callback)
    {
        (int, int) result = (0, 0);

        yield return panel.RollDiceSequence((d1, d2) =>
        {
            result = (d1, d2);
        });

        HandleDiceResult(result, callback);
    }

    public virtual IEnumerator OptBuyProperty(PropertyTile property, System.Action<bool> callback)
    {
        int sellPrice = property.GetSellPrice();
        if (sellPrice > balance)
        {
            callback.Invoke(false);
            yield break;
        }

        yield return panel.BuyPropertySequence(sellPrice, callback);
    }

    public virtual IEnumerator OptBuildHouse(PropertyTile property, System.Action<bool> callback)
    {
        int housePrice = property.GetHousePrice();
        if (housePrice > balance)
        {
            callback.Invoke(false);
            yield break;
        }


        yield return panel.BuildHouseSequence(housePrice, callback);
    }

    public virtual IEnumerator OptSelectTile(List<Tile> tiles, System.Action<Tile> callback)
    {
        Ray ray;
        Tile chosenTile = null;

        while (true)
        {
            if (Mouse.current == null)
            {
                Debug.LogWarning("Mouse not detected by new Input System");
                yield break;
            }

            Vector2 mousePos = Mouse.current.position.ReadValue();
            ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject hoveredObject = hit.collider.gameObject;

                foreach (Tile tile in tiles)
                {
                    if (tile.gameObject == hoveredObject)
                    {
                        if (Mouse.current.leftButton.wasPressedThisFrame)
                        {
                            chosenTile = tile;
                            break;
                        }
                    }
                }

                if (chosenTile != null) break;
            }

            yield return null;
        }

        callback.Invoke(chosenTile);
        yield break;
    }

    public virtual IEnumerator OptShowCard(Card card)
    {
        yield return panel.ShowCardSequence(card, false);
    }

    public virtual IEnumerator OptSellProperty()
    {
        yield break;
    }

    void Awake()
    {
        playerCollider = GetComponent<CapsuleCollider>();

        GameObject panelObj = Instantiate(panelPrefab);
        panel = panelObj.GetComponent<PlayerPanel>();
    }

    void Start()
    {
        modelRenderer.material.color = Random.ColorHSV();
    }

    void Update()
    {
        UpdatePosition();
        UpdatePanel();
    }
}
