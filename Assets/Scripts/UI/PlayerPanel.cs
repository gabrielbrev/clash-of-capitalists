using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    private static readonly WaitForSeconds _waitForSeconds1_5 = new(1.5f);
    [SerializeField] Text infoText;
    [SerializeField] Text diceResultText;
    [SerializeField] private RollDicePanel rollDicePanel;
    [SerializeField] private BuyPropertyPanel buyPropertyPanel;

    public void SetInfo(string text)
    {
        infoText.text = text;
    }

    public IEnumerator RollDiceSequence(Action<int, int> callback)
    {
        rollDicePanel.Show();

        yield return rollDicePanel.WaitForDiceRoll(callback);

        rollDicePanel.Hide();
    }

    public IEnumerator BuyPropertySequence(int propertyValue, Action<bool> callback)
    {
        buyPropertyPanel.Show();

        buyPropertyPanel.SetPropertyValue(propertyValue);
        yield return buyPropertyPanel.WaitForDecision(callback);

        buyPropertyPanel.Hide();
    }

    public IEnumerator SetDiceResult((int, int) result)
    {
        diceResultText.text = $"{result.Item1} + {result.Item2} = {result.Item1 + result.Item2}";
        diceResultText.enabled = true;

        yield return _waitForSeconds1_5;

        diceResultText.enabled = false;
    }

    void Awake()
    {
        rollDicePanel.Hide();
        buyPropertyPanel.Hide();
    }

    void Start()
    {
        diceResultText.enabled = false;
    }
}