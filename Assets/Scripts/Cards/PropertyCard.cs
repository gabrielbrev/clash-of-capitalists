using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PropertyCard : Card
{
    protected abstract List<PropertyTile> GetAvailabelProperties(Player player);

    protected abstract IEnumerator Execute(Player player, PropertyTile property);

    public override IEnumerator Use(Player player)
    {
        List<PropertyTile> properties = GetAvailabelProperties(player);

        if (properties.Count == 0) yield break;

        PropertyTile chosenTile = null;
        yield return player.OptSelectTile(properties, (tile) => chosenTile = tile);
        yield return new WaitUntil(() => chosenTile != null);
        yield return Execute(player, chosenTile);
    }
}