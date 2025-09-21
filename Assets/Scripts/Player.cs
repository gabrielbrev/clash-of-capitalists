using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    private int index;
    private Renderer playerRenderer;
    private CapsuleCollider playerCollider;

    public int GetIndex()
    {
        return index;
    }

    public CapsuleCollider GetCollider()
    {
        return playerCollider;
    }

    public void MoveTo(Tile tile)
    {
        Vector3 tilePos = tile.transform.position;
        transform.position = new(
            tilePos.x,
            tilePos.y + playerCollider.bounds.size.y / 2,
            tilePos.z
        );

        index = tile.GetIndex();
    }

    void Awake()
    {
        playerCollider = GetComponent<CapsuleCollider>();
        playerRenderer = GetComponent<Renderer>();
    }

    void Start()
    {
        playerRenderer.material.color = Utils.GetRandomColor();
    }

    void Update()
    {
        
    }
}
