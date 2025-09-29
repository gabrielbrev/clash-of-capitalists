using System.Collections;
using UnityEngine;

public class StartTile : Tile
{
    [SerializeField] private int bonusAmount;

    private void GiveBonus(Player player)
    {
        player.AddBalance(bonusAmount);
    }

    public override IEnumerator PassBy(Player player)
    {
        GiveBonus(player);
        yield break;
    }

    public override IEnumerator Visit(Player player)
    {
        GiveBonus(player);
        yield break;
    }
}