using System.Linq;
using UnityEngine;

public class Monopoly : MonoBehaviour
{
    [SerializeField] private Color color;
    [SerializeField] private PropertyTile[] properties;

    public bool IsMonopolyOwner(Player player)
    {
        foreach (PropertyTile prop in properties)
        {
            if (!prop.IsOwner(player)) return false;
        }

        return true;
    }

    public int GetMinNumHouses()
    {
        return properties.Min(prop => prop.GetNumHouses());
    }

    public Color GetColor()
    {
        return color;
    }

    void Awake()
    {
        foreach (PropertyTile prop in properties)
        {
            prop.SetMonopoly(this);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
