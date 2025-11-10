using System.Collections;
using System.Collections.Generic;

public abstract class PropertyCard : Card
{
    protected abstract List<PropertyTile> GetAvailabelProperties(Player player);

    protected abstract void Execute(Player player, PropertyTile property);

    public override IEnumerator Use(Player player)
    {
        List<PropertyTile> properties = GetAvailabelProperties(player);

        if (properties.Count == 0) yield break;
        
        yield return player.OptSelectTile(properties, (chosenTile) =>
        {
            Execute(player, chosenTile);
        });
    }
}