using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceMovementCard : MovementCard
{
    public override IEnumerator DetermineDestination(Player player, Action<Tile> callback)
    {
        List<Tile> tiles = (List<Tile>)Tile.GetSelectableTiles(player);
        yield return player.OptSelectTile(tiles, callback);
    }
}