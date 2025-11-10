using System.Collections;
using UnityEngine;

public class CopTile : Tile
{
    private static readonly WaitForSeconds _waitForSeconds0_75 = new(0.75f);
    [SerializeField] int prisonTime;
    [SerializeField] PrisonTile prison;

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
        player.SetPrisonRounds(prisonTime);

        yield return _waitForSeconds0_75;
        player.MoveTo(prison);
    }
}
