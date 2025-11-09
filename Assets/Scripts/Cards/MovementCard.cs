using System;
using System.Collections;

public abstract class MovementCard : Card
{
    public abstract IEnumerator DetermineDestination(Player player, Action<Tile> callback);

    override public IEnumerator Use(Player player)
    {
        yield return DetermineDestination(player, (chosenTile) =>
        {
            player.MoveTo(chosenTile);
        });
    }
}