using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAI : Player
{
    private static readonly WaitForSeconds _waitForSeconds1 = new(1f);

    public override IEnumerator OptRollDice(Action<(int result, bool equalValues)> callback)
    {
        HandleDiceResult(Dice.Roll(), callback);
        yield break;
    }

    public override IEnumerator OptBuyProperty(PropertyTile property, Action<bool> callback)
    {
        yield return _waitForSeconds1;
        int sellPrice = property.GetSellPrice();
        callback.Invoke(sellPrice <= balance * 0.85);
    }

    public override IEnumerator OptBuildHouse(PropertyTile property, Action<bool> callback)
    {
        yield return _waitForSeconds1;
        int buildPrice = property.GetSellPrice();
        callback.Invoke(buildPrice <= balance * 0.4);
    }

    public override IEnumerator OptShowCard(Card card)
    {
        yield return panel.ShowCardSequence(card, true);
    }

    public override IEnumerator OptSelectTile<T>(List<T> tiles, Action<T> callback)
    {
        System.Random random = new();
        callback?.Invoke(tiles[random.Next(tiles.Count)]);
        yield break;
    }
}