using System.Collections;
using TMPro;
using UnityEngine;

public class PropertyTile : Tile
{
    private static readonly int MAX_HOUSES = 4;
    [SerializeField] private GameObject[] houses;
    [SerializeField] private GameObject marker;
    [SerializeField] private GameObject colorPlane;
    [SerializeField] private TextMeshPro headerText;
    [SerializeField] private TextMeshPro bodyText;
    [SerializeField] private string propertyName;
    [SerializeField] private int basePrice;
    private Player owner;
    private int numHouses;
    private Monopoly monopoly;

    public bool IsOwner(Player player)
    {
        return player == owner;
    }

    public IEnumerator Buy(Player player)
    {
        int sellPrice = GetSellPrice();
        if (player.GetNetWorth() < sellPrice) yield break;

        yield return player.SubtractBalance(sellPrice);

        Renderer markerRenderer = marker.GetComponent<Renderer>();
        markerRenderer.material.color = player.GetColor();
        marker.SetActive(true);

        owner = player;
        owner.AddProperty(this);
    }

    public void Auction()
    {
        // TODO
    }

    public void Sell()
    {
        owner.AddBalance(GetSellPrice());
        owner.RemoveProperty(this);
        owner = null;
        marker.SetActive(false);
    }

    public IEnumerator BuildHouse()
    {
        int buildPrice = GetHousePrice();
        if (owner.GetNetWorth() < buildPrice) yield break;

        yield return owner.SubtractBalance(buildPrice);
        numHouses += 1;

        for (int i = 0; i < numHouses; i++)
        {
            GameObject house = houses[i];
            Renderer renderer = house.GetComponent<Renderer>();
            renderer.material.color = owner.GetColor();
            house.SetActive(true);
        }

        marker.SetActive(false);
    }

    public void SetMonopoly(Monopoly monopoly)
    {
        this.monopoly = monopoly;
    }
    
    public int GetHousePrice()
    {
        return (int)(basePrice * (0.2 + numHouses * 0.15));
    }

    public int GetBasePrice()
    {
        return basePrice;
    }

    public int GetSellPrice()
    {
        float summedBasePrices = basePrice + numHouses * GetHousePrice();
        return (int)(owner ? summedBasePrices * 1.5 : summedBasePrices);
    }

    public int GetNumHouses()
    {
        return numHouses;
    }

    public int GetRentPrice()
    {
        int sellPrice = GetSellPrice();
        return (int)(sellPrice * 0.475);
    }


    public override IEnumerator PassBy(Player player)
    {
        yield break;
    }

    public override IEnumerator Visit(Player player)
    {
        if (!owner)
        {
            yield return player.OptBuyProperty(this, (buy) =>
            {
                if (buy) StartCoroutine(Buy(player));
            });
        }
        else if (player == owner)
        {
            if ((!monopoly || monopoly.IsMonopolyOwner(player)) && numHouses < MAX_HOUSES)
            {
                yield return player.OptBuildHouse(this, (build) =>
                {
                    if (build) StartCoroutine(BuildHouse());
                });
            }
        }
        else
        {
            int rentPrice = GetRentPrice();
            yield return player.SubtractBalance(rentPrice);
            owner.AddBalance(rentPrice);
        }

        yield break;
    }

    override protected void Start()
    {
        base.Start();

        if (monopoly)
        {
            Renderer renderer = colorPlane.GetComponent<Renderer>();
            renderer.material.color = monopoly.GetColor();
        }

        foreach (GameObject house in houses)
        {
            house.SetActive(false);
        }

        marker.SetActive(false);
    }

    void Update()
    {
        headerText.text = propertyName;
        bodyText.text = !owner ? $"À venda:\nR$ {GetBasePrice() / 1000f:F1}k" : $"Aluguel:\nR$ {GetRentPrice() / 1000f:F1}k";
    }
}
