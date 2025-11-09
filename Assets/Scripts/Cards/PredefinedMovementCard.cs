using System;
using System.Collections;
using UnityEngine;

public class PredefinedMovementCard : MovementCard
{
    [SerializeField] private Tile destination;

    public override IEnumerator DetermineDestination(Player player, Action<Tile> callback)
    {
        callback?.Invoke(destination);
        yield break;
    }
}