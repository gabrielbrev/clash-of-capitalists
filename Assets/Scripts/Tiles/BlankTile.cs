using System.Collections;

public class BlankTile : Tile
{
    public override IEnumerator PassBy(Player player)
    {
        yield break;
    }

    public override IEnumerator Visit(Player player)
    {
        yield break;
    }
}