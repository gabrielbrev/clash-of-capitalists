using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BuyPropertyCard : PropertyCard
{
    protected override IEnumerator Execute(Player player, PropertyTile property)
    {
        yield return property.Buy(player);
    }

    protected override List<PropertyTile> GetAvailabelProperties(Player player)
    {
        IReadOnlyList<PropertyTile> properties = PropertyTile.GetAll();
        return properties.Where(p => !p.IsOwner(player) && p.GetBasePrice() < player.GetBalance()).ToList();
    }
}