using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    private static readonly WaitForSeconds _waitForSeconds1_5 = new(1.5f);
    private Vector2 _balanceInitialPosition;
    [SerializeField] private Text infoText;
    [SerializeField] private Text diceResultText;
    [SerializeField] private Text balanceText;
    [SerializeField] private Text alertText;
    [SerializeField] private RollDicePanel rollDicePanel;
    [SerializeField] private DecisionPanel decisionPanel;
    [SerializeField] private ShowCardPanel showCardPanel;

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

    public void SetAlertText(string text) {
        if (text.Length > 0)
        {
            alertText.enabled = true;
            alertText.text = text;
        }
        else
        {
            alertText.enabled = false;
        }
    }

    public IEnumerator RollDiceSequence(Action<int, int> callback)
    {
        rollDicePanel.Show();

        yield return rollDicePanel.WaitForDiceRoll(callback);

        rollDicePanel.Hide();
    }

    public IEnumerator BuyPropertySequence(int propertyPrice, Action<bool> callback)
    {
        decisionPanel.Show();

        decisionPanel.SetQuestionText($"Deseja comprar essa propriedade por {propertyPrice:C}?");
        yield return decisionPanel.WaitForDecision(callback);

        decisionPanel.Hide();
    }

    public IEnumerator BuildHouseSequence(int housePrice, Action<bool> callback)
    {
        decisionPanel.Show();

        decisionPanel.SetQuestionText($"Deseja construir uma casa por {housePrice:C}?");
        yield return decisionPanel.WaitForDecision(callback);

        decisionPanel.Hide();
    }

    public IEnumerator ShowCardSequence(Card card, bool autoclose)
    {
        showCardPanel.Show();

        yield return showCardPanel.WaitForConfirmation(card, autoclose);

        showCardPanel.Hide();
    }

    void Awake()
    {
        rollDicePanel.Hide();
        decisionPanel.Hide();
        showCardPanel.Hide();
        _balanceInitialPosition = balanceText.rectTransform.anchoredPosition;
    }

    void Start()
    {
        diceResultText.enabled = false;
        balanceText.enabled = false;
        alertText.enabled = false;
    }
}