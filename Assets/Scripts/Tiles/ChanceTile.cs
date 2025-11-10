using System.Collections;
using UnityEngine;

public class ChanceTile : Tile
{
    private static readonly System.Random rng = new();
    private Card[] cards;
    [SerializeField] private GameObject cardsFolder;

    public override bool IsSelectable(Player player)
    {
        return false;
    }

    public override IEnumerator PassBy(Player player)
    {
        yield break;
    }

    public override IEnumerator Visit(Player player)
    {
        Card randomCard = cards[rng.Next(cards.Length)];
        yield return player.OptShowCard(randomCard);
        yield return randomCard.Use(player);
    }

    protected override void Awake()
    {
        base.Awake();
        cards = cardsFolder.GetComponentsInChildren<Card>(true);
    }
}
