using System.Collections;
using UnityEngine;

public class ChanceTile : Tile
{
    [SerializeField] private Card[] cards;
    private static readonly System.Random rng = new();


    public override IEnumerator PassBy(Player player)
    {
        yield break;
    }

    public override IEnumerator Visit(Player player)
    {
        Card randomCard = cards[rng.Next(cards.Length)];
        yield return randomCard.Use(player);
    }
}
