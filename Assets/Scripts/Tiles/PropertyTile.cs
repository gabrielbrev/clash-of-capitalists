using System.Collections;
using UnityEngine;

public class PropertyTile : Tile
{
    // [SerializeField] private Monopoly monopoly;
    [SerializeField] private string propertyName;
    [SerializeField] private int basePrice;
    private Player owner;
    private int numHouses;

    public bool IsOwner(Player player)
    {
        return player == owner;
    }

    public IEnumerator Buy(Player player)
    {
        int sellPrice = GetSellPrice();
        if (player.GetNetWorth() < sellPrice) yield break;

        yield return player.SubtractBalance(sellPrice);
        owner = player;
    }

    public void Auction()
    {
        // TODO
    }

    public void Sell()
    {
        owner.AddBalance(GetSellPrice());
        owner = null;
    }

    public IEnumerator BuildHouse()
    {
        int buildPrice = GetHousePrice();
        if (owner.GetNetWorth() < buildPrice) yield break;

        yield return owner.SubtractBalance(buildPrice);
        numHouses += 1;
    }
    
    public int GetHousePrice()
    {
        return (int)(basePrice * (0.2 + numHouses * 0.025));
    }

    public int GetBasePrice()
    {
        return basePrice;
    }

    public int GetSellPrice()
    {
        float summedBasePrices = basePrice + numHouses * GetHousePrice();
        return (int)(owner ? summedBasePrices * 1.3 : summedBasePrices);
    }

    public int GetNumHouses()
    {
        return numHouses;
    }

    public int GetRentPrice()
    {
        int sellPrice = GetSellPrice();
        return (int)(sellPrice * 0.075);
    }


    public override IEnumerator PassBy(Player player)
    {
        yield break;
    }

    public override IEnumerator Visit(Player player)
    {
        if (!owner)
        {
            yield return player.OptBuyProperty(this);
        }
        else if (player == owner)
        {
            yield return player.OptBuildHouse(this);
        }
        else
        {
            int rentPrice = GetRentPrice();
            yield return player.SubtractBalance(rentPrice);
            owner.AddBalance(rentPrice);
        }
        
        yield break;
    }
}
