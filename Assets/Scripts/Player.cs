using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private int index;
    private Tile currTile;
    private Renderer playerRenderer;
    private CapsuleCollider playerCollider;
    private Vector3 targetPos;

    private void UpdatePosition()
    {
        if (!currTile) return;
        targetPos = currTile.GetPlayerPosition(this);
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

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
        if (currTile) currTile.RemovePlayer(this);
        tile.AddPlayer(this);

        currTile = tile;
        index = tile.GetIndex();
    }

    void Awake()
    {
        playerCollider = GetComponent<CapsuleCollider>();
        playerRenderer = GetComponent<Renderer>();
    }

    void Start()
    {
        playerRenderer.material.color = Random.ColorHSV();
    }

    void Update()
    {
        UpdatePosition();
    }
}
