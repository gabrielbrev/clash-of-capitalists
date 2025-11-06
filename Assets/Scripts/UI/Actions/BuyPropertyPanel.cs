using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuyPropertyPanel : ActionPanel
{
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;
    [SerializeField] Text questionText;

    public void SetPropertyValue(int value)
    {
        questionText.text = $"Deseja comprar essa propriedade por {value:C}?";   
    }

    public IEnumerator WaitForDecision(Action<bool> onDecision)
    {
        bool decided = false;

        void yesAction()
        {
            onDecision?.Invoke(true);
            decided = true;
        }

        void noAction()
        {
            onDecision?.Invoke(false);
            decided = true;
        }

        yesButton.onClick.AddListener(yesAction);
        noButton.onClick.AddListener(noAction);

        yield return new WaitUntil(() => decided);

        yesButton.onClick.RemoveListener(yesAction);
        noButton.onClick.RemoveListener(noAction);

    }

    protected override void OnShow()
    {
        yesButton.interactable = true;
        noButton.interactable = true;
    }

    protected override void OnHide()
    {
        yesButton.interactable = false;
        noButton.interactable = false;
    }
}