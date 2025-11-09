using System.Collections;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    [SerializeField] private string cardName;
    [SerializeField] private string description;

    public abstract IEnumerator Use(Player player);

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
