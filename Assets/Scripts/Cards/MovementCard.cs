using System;
using System.Collections;
using UnityEngine;

public abstract class MovementCard : Card
{
    public abstract IEnumerator DetermineDestination(Player player, Action<Tile> callback);

    override public IEnumerator Use(Player player)
    {
        Tile tile = null;

        yield return DetermineDestination(player, (chosenTile) =>
        {
            player.MoveTo(chosenTile);
            tile = chosenTile;
        });

        yield return new WaitUntil(() => tile != null);
        yield return tile.Visit(player);
    }
}