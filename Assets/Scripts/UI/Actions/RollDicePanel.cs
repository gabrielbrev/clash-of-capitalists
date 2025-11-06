using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RollDicePanel : ActionPanel
{
    [SerializeField] Button button;

    public IEnumerator WaitForDiceRoll(Action<int, int> onDiceRoll)
    {
        bool clicked = false;

        void action()
        {
            var (d1, d2) = Dice.Roll();
            onDiceRoll?.Invoke(d1, d2);
            clicked = true;
        }

        button.onClick.AddListener(action);
        yield return new WaitUntil(() => clicked);
        button.onClick.RemoveListener(action);
    }

    protected override void OnShow() => button.interactable = true;
    protected override void OnHide() => button.interactable = false;
}