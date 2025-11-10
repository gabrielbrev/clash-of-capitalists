using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    private static readonly WaitForSeconds _waitForSeconds5 = new(5f);
    private static readonly List<Player> instances = new();
    public Renderer modelRenderer;
    [SerializeField] private GameObject panelPrefab;
    [SerializeField] private float moveSpeed;
    [SerializeField] protected int balance;
    private int index;
    private int prisonTime;
    private Tile currTile;
    private bool bankrupt;
    private readonly List<PropertyTile> properties = new();
    private CapsuleCollider playerCollider;
    private Vector3 targetPos;
    protected PlayerPanel panel;

    public static IReadOnlyList<Player> GetAll()
    {
        return instances.ToList();
    }

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

    public void RemoveProperty(PropertyTile property)
    {
        properties.Remove(property);
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
        while (balance < amount)
        {
            if (properties.Count <= 0)
            {
                bankrupt = true;
                panel.SetAlertText("Você faliu.");
                yield return _waitForSeconds5;
                yield break;
            }

            panel.SetAlertText("Você não tem dinheiro suficiente. Escolha uma propriedade para vender.");

            yield return OptSelectTile(properties, (chosenTile) =>
            {
                chosenTile.Sell();
            });
        }

        panel.SetAlertText("");

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

    public int GetBalance()
    {
        return balance;
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

    public bool IsBankrupt()
    {
        return bankrupt;
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

    public virtual IEnumerator OptSelectTile<T>(List<T> tiles, System.Action<T> callback) where T : Tile
    {
        Ray ray;
        T chosenTile = null;

        foreach (T tile in tiles)
        {
            tile.EnableGlow(Color.cyan, 0.5f);
        }

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

                foreach (T tile in tiles)
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

        foreach (T tile in tiles)
        {
            tile.DisableGlow();
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
        instances.Add(this);
        playerCollider = GetComponent<CapsuleCollider>();

        GameObject panelObj = Instantiate(panelPrefab);
        panel = panelObj.GetComponent<PlayerPanel>();
    }

    void Start()
    {
        bankrupt = false;
        modelRenderer.material.color = Random.ColorHSV();
    }

    void Update()
    {
        UpdatePosition();
        UpdatePanel();
    }

    void OnDestroy()
    {
        instances.Remove(this);
    }
}
