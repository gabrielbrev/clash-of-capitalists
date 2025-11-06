using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    [SerializeField] private GameObject panelPrefab;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int balance;
    private PlayerPanel panel;
    private int index;
    private int prisonTime;
    private Tile currTile;
    private List<PropertyTile> properties = new();

    private Renderer playerRenderer;
    private CapsuleCollider playerCollider;
    private Vector3 targetPos;

    private void UpdatePosition()
    {
        if (!currTile) return;
        targetPos = currTile.GetPlayerPosition(this);
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    private void UpdatePanel()
    {
        panel.SetInfo(
            $"{name}\n{balance:C}\nPosição {index}\nTempo de prisão: {prisonTime}"
        );
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

    public IEnumerator OptRollDice(System.Action<(int result, bool equalValues)> callback)
    {
        (int, int) result = (0, 0);

        yield return panel.RollDiceSequence((d1, d2) =>
        {
            result = (d1, d2);
        });

        StartCoroutine(panel.SetDiceResult(result));

        if (result.Item1 == result.Item2) prisonTime = 0;

        callback.Invoke((
            prisonTime > 0 ? 0 : result.Item1 + result.Item2,
            result.Item1 == result.Item2
        ));

        if (prisonTime > 0) prisonTime--;
    }

    public IEnumerator OptBuyProperty(PropertyTile property)
    {
        int sellPrice = property.GetSellPrice();
        if (sellPrice > balance) yield break;

        bool buy = false; // Initialize with any value bcs of type checking

        yield return panel.BuyPropertySequence(sellPrice, (decision) => buy = decision);

        if (buy) {
            yield return property.Buy(this);
            properties.Add(property);
        }
    }

    public IEnumerator OptBuildHouse(PropertyTile property)
    {
        yield break;
    }

    public IEnumerator OptSelectTile(List<Tile> tiles, System.Action<Tile> onSelect)
    {
        yield break;
    }

    public IEnumerator OptSellProperty()
    {
        yield break;
    }

    void Awake()
    {
        playerCollider = GetComponent<CapsuleCollider>();
        playerRenderer = GetComponent<Renderer>();

        GameObject panelObj = Instantiate(panelPrefab);
        panel = panelObj.GetComponent<PlayerPanel>();
    }

    void Start()
    {
        playerRenderer.material.color = Random.ColorHSV();
    }

    void Update()
    {
        UpdatePosition();
        UpdatePanel();
    }
}
