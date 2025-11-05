using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RollDicePanel : ActionPanel
{
    [SerializeField] Button button;

    private (int, int) RollDice()
    {
        return (UnityEngine.Random.Range(1, 7), UnityEngine.Random.Range(1, 7));
    }

    public IEnumerator WaitForDiceRoll(Action<int, int> onDiceRoll)
    {
        bool clicked = false;

        void action()
        {
            var (d1, d2) = RollDice();
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