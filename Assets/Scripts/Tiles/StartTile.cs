using UnityEngine;

public class StartTile : Tile
{
    [SerializeField] private int bonusAmount;

    private void GiveBonus(Player player)
    {
        player.AddBalance(bonusAmount);
    }

    public override void PassBy(Player player)
    {
        GiveBonus(player);
    }

    public override void Visit(Player player)
    {
        GiveBonus(player);
    }
}