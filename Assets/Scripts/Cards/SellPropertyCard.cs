using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SellPropertyCard : PropertyCard
{
    protected override IEnumerator Execute(Player player, PropertyTile property)
    {
        property.Sell();
        yield break; 
    }

    protected override List<PropertyTile> GetAvailabelProperties(Player player)
    {
        IReadOnlyList<PropertyTile> properties = PropertyTile.GetAll();
        return properties.Where(p => p.IsOwner(player)).ToList();
    }
}