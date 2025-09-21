using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    private int index;
    protected bool selectable;

    public int GetIndex()
    {
        return index;
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }

    public bool IsSelectable()
    {
        return selectable;
    }

    public abstract void PassBy(Player player);

    public abstract void Visit(Player player);

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
