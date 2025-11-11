using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PropertyTile : Tile
{
    private static readonly int MAX_HOUSES = 4;
    private static readonly List<PropertyTile> instances = new();
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

    public static new IReadOnlyList<PropertyTile> GetAll()
    {
        return instances.ToList();
    }

    public bool IsOwner(Player player)
    {
        return player == owner;
    }

    public override bool IsSelectable(Player player)
    {
        return owner == null || player == owner;
    }

    public IEnumerator Buy(Player player)
    {
        int sellPrice = GetSellPrice();
        if (player.GetNetWorth() < sellPrice) yield break;

        yield return player.SubtractBalance(sellPrice);

        if (owner) owner.AddBalance(sellPrice);

        owner = player;
        owner.AddProperty(this);

        if (numHouses == 0) marker.SetActive(true);
        UpdateOwnerColors();
    }

    private void UpdateOwnerColors()
    {
        if (!owner) return;

        Color ownerColor = owner.GetColor();

        Renderer markerRenderer = marker.GetComponent<Renderer>();
        markerRenderer.material.color = ownerColor;

        for (int i = 0; i < numHouses; i++)
        {
            GameObject house = houses[i];
            Renderer renderer = house.GetComponent<Renderer>();
            renderer.material.color = ownerColor;
        }
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

        houses[numHouses - 1].SetActive(true);
        UpdateOwnerColors();
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
        
        instances.Add(this);

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

    protected override void OnDestroy()
    {
        base.OnDestroy();

        instances.Remove(this);
    }
}
