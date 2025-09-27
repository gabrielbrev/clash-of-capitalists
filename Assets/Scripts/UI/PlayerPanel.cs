using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField] Text infoText;
    [SerializeField] Text diceResultText;
    [SerializeField] Button rollButton;
    private Action rollButtonAction;
    public void SetInfo(string text)
    {
        infoText.text = text;
    }

    public void SetRoolButtonInteractable(bool interactable)
    {
        rollButton.interactable = interactable;
    }

    public void SetRollButtonAction(Action action)
    {
        rollButtonAction = null;
        rollButtonAction += action;
    }

    public IEnumerator SetDiceResult(int value)
    {
        diceResultText.text = value.ToString();
        diceResultText.enabled = true;

        yield return new WaitForSeconds(value);

        diceResultText.enabled = false;
    }

    void Start()
    {
        rollButton.onClick.AddListener(() =>
        {
            rollButtonAction?.Invoke();
        });
        diceResultText.enabled = false;
    }

    void Update()
    {

    }
}
