using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public abstract class Tile : MonoBehaviour
{
    private static readonly List<Tile> instances = new();
    private int index;
    private readonly List<Player> players = new();
    protected Renderer tileRenderer;

    public static IReadOnlyList<Tile> GetSelectableTiles(Player player)
    {
        return instances.Where(t => t.IsSelectable(player)).ToList();
    }

    public static IReadOnlyList<Tile> GetAll()
    {
        return instances.ToList();
    }

    public int GetIndex()
    {
        return index;
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }

    public virtual bool IsSelectable(Player player)
    {
        return true;
    }

    public void AddPlayer(Player player)
    {
        players.Add(player);
    }

    public void RemovePlayer(Player player)
    {
        players.Remove(player);
    }

    public Vector3 GetPlayerPosition(Player player)
    {
        Vector3 tilePos = transform.position;
        Vector3 centeredPlayerPos = new(
            tilePos.x,
            tilePos.y + player.GetCollider().bounds.size.y / 2,
            tilePos.z
        );

        if (players.Count <= 1)
        {
            return centeredPlayerPos;
        }
        else
        {
            // Divide os players em um circulo dentro do tile para que nenhum sobreponha o outro
            int playerIndex = players.FindIndex(p => p == player);
            float angleDegrees = 360f / players.Count * playerIndex;
            float radius = tileRenderer.bounds.size.x * 1f / 5f;
            
            Vector3 boardCenterDirection = transform.parent.position - tilePos;
            float angleToCenter = Vector3.Angle(Vector3.forward, boardCenterDirection) + 90f;

            Quaternion rotation = Quaternion.Euler(0, angleDegrees + angleToCenter, 0);
            Vector3 direction = rotation * Vector3.forward;

            return centeredPlayerPos + direction * radius;
        }
    }

    public abstract IEnumerator PassBy(Player player);

    public abstract IEnumerator Visit(Player player);

    void Awake()
    {
        instances.Add(this);
        tileRenderer = GetComponent<Renderer>();
    }

    protected virtual void Start()
    {
        Debug.Log(name);
        if (tileRenderer.material.mainTexture == null) tileRenderer.material.color = Random.ColorHSV();
    }

    void OnDestroy()
    {
        instances.Remove(this);
    }
}
