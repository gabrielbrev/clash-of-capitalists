using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DecisionPanel : ActionPanel
{
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;
    [SerializeField] Text questionText;

    public void SetQuestionText(string text)
    {
        questionText.text = text;   
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