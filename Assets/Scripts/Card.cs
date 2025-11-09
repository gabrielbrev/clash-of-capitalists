using System.Collections;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    [SerializeField] private string cardName;
    [SerializeField] private string description;

    public string GetName()
    {
        return cardName;
    }

    public string GetDescription()
    {
        return description;
    }

    public void SetDescription(string text)
    {
        description = text;
    }

    public abstract IEnumerator Use(Player player);
}
