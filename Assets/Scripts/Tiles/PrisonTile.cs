using System.Collections;
using UnityEngine;

public class PrisonTile : Tile
{
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
        yield break;
    }
}
