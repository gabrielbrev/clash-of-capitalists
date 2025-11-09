using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShowCardPanel : ActionPanel
{
    [SerializeField] Button button;
    [SerializeField] Text nameText;
    [SerializeField] Text descriptionText;

    public IEnumerator WaitForConfirmation(Card card)
    {
        nameText.text = card.GetName();
        descriptionText.text = card.GetDescription();

        bool clicked = false;

        void action()
        {
            clicked = true;
        }

        button.onClick.AddListener(action);
        yield return new WaitUntil(() => clicked);
        button.onClick.RemoveListener(action);
    }

    protected override void OnShow() => button.interactable = true;
    protected override void OnHide() => button.interactable = false;
}