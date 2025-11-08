using System.Collections;
using UnityEngine;


public class PlayerAI : Player
{
    private static readonly WaitForSeconds _waitForSeconds1 = new(1f);

    public override IEnumerator OptRollDice(System.Action<(int result, bool equalValues)> callback)
    {
        HandleDiceResult(Dice.Roll(), callback);
        yield break;
    }

    public override IEnumerator OptBuyProperty(PropertyTile property)
    {
        yield return _waitForSeconds1;
        int sellPrice = property.GetSellPrice();
        if (sellPrice > balance * 0.85) yield break;
        else yield return BuyProperty(property);
    }

    public override IEnumerator OptBuildHouse(PropertyTile property)
    {
        yield return _waitForSeconds1;
        int buildPrice = property.GetSellPrice();
        if (buildPrice > balance * 0.4) yield break;
        else yield return property.BuildHouse();
    }
}