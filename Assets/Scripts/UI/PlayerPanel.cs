using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    private static readonly WaitForSeconds _waitForSeconds1_5 = new(1.5f);
    private Vector2 _balanceInitialPosition;
    [SerializeField] Text infoText;
    [SerializeField] Text diceResultText;
    [SerializeField] Text balanceText;
    [SerializeField] private RollDicePanel rollDicePanel;
    [SerializeField] private BuyPropertyPanel buyPropertyPanel;
    [SerializeField] private BuildHousePanel buildHousePanel;

    private IEnumerator AnimateBalanceText()
    {
        float duration = 0.5f;
        float elapsedTime = 0f;
        Vector2 startPosition = _balanceInitialPosition + new Vector2(0, -100);
        balanceText.rectTransform.anchoredPosition = startPosition;
        balanceText.enabled = true;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            balanceText.rectTransform.anchoredPosition = Vector2.Lerp(startPosition, _balanceInitialPosition, t);
            yield return null;
        }

        balanceText.enabled = false;
    }

    public void SetInfo(string text)
    {
        infoText.text = text;
    }

    public IEnumerator SetDiceResult((int, int) result)
    {
        diceResultText.text = $"{result.Item1} + {result.Item2} = {result.Item1 + result.Item2}";
        diceResultText.enabled = true;

        yield return _waitForSeconds1_5;

        diceResultText.enabled = false;
    }

    public void SetBalanceText(float amount)
    {
        balanceText.text = amount.ToString("C");
        StartCoroutine(AnimateBalanceText());
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

    public IEnumerator BuildHouseSequence(int houseValue, Action<bool> callback)
    {
        buildHousePanel.Show();

        buildHousePanel.SetHouseValue(houseValue);
        yield return buildHousePanel.WaitForDecision(callback);

        buildHousePanel.Hide();
    }

    void Awake()
    {
        rollDicePanel.Hide();
        buyPropertyPanel.Hide();
        buildHousePanel.Hide();
        _balanceInitialPosition = balanceText.rectTransform.anchoredPosition;
    }

    void Start()
    {
        diceResultText.enabled = false;
        balanceText.enabled = false;
    }
}