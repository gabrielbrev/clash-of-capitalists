using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    public Renderer modelRenderer;
    [SerializeField] private GameObject panelPrefab;
    [SerializeField] private float moveSpeed;
    [SerializeField] protected int balance;
    private PlayerPanel panel;
    private int index;
    private int prisonTime;
    private Tile currTile;
    private readonly List<PropertyTile> properties = new();

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

    protected IEnumerator BuyProperty(PropertyTile property)
    {
        yield return property.Buy(this);
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

    public virtual IEnumerator OptBuyProperty(PropertyTile property)
    {
        int sellPrice = property.GetSellPrice();
        if (sellPrice > balance) yield break;

        bool buy = false; // Initialize with any value bcs of type checking

        yield return panel.BuyPropertySequence(sellPrice, (decision) => buy = decision);

        if (buy) yield return BuyProperty(property);
    }

    public virtual IEnumerator OptBuildHouse(PropertyTile property)
    {
        int housePrice = property.GetHousePrice();
        if (housePrice > balance) yield break;

        bool build = false; // Initialize with any value bcs of type checking

        yield return panel.BuildHouseSequence(housePrice, (decision) => build = decision);

        if (build) yield return property.BuildHouse();
    }

    public virtual IEnumerator OptSelectTile(List<Tile> tiles, System.Action<Tile> onSelect)
    {
        yield break;
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
