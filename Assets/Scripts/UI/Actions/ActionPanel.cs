using UnityEngine;

public abstract class ActionPanel : MonoBehaviour
{
    public virtual void Show()
    {
        gameObject.SetActive(true);
        OnShow();
    }

    public virtual void Hide()
    {
        OnHide();
        gameObject.SetActive(false);
    }

    protected abstract void OnShow();
    protected abstract void OnHide();
}