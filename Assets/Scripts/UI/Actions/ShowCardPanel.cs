using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShowCardPanel : ActionPanel
{
    private static WaitForSeconds _waitForSeconds2_5 = new(2.5f);
    [SerializeField] Button button;
    [SerializeField] Text nameText;
    [SerializeField] Text descriptionText;

    public IEnumerator WaitForConfirmation(Card card, bool autoclose)
    {
        nameText.text = card.GetName();
        descriptionText.text = card.GetDescription();

        if (autoclose)
        {
            button.enabled = false;

            yield return _waitForSeconds2_5;

            button.enabled = true;
        } else
        {
            bool clicked = false;

            void action()
            {
                clicked = true;
            }

            button.onClick.AddListener(action);
            yield return new WaitUntil(() => clicked);
            button.onClick.RemoveListener(action);
        }
    }

    protected override void OnShow() => button.interactable = true;
    protected override void OnHide() => button.interactable = false;
}