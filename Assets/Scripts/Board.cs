using System.Drawing;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Board : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private int rows;
    [SerializeField] private int cols;
    [SerializeField] private Tile[] tiles;

    private Renderer boardRenderer;

    private bool IsCorner(Vector2Int vec)
    {
        int maxX = rows - 1;
        int maxY = cols - 1;

        return (vec.x == 0 && vec.y == 0)
            || (vec.x == 0 && vec.y == maxY)
            || (vec.x == maxX && vec.y == 0)
            || (vec.x == maxX && vec.y == maxY);
    }

    private void SetTilePosition(Tile tile, Vector2Int position, Renderer tileRenderer)
    {
        float tileSizeX = tileRenderer.bounds.size.x;
        float tileSizeZ = tileRenderer.bounds.size.z;

        tile.transform.position = new(
            boardRenderer.bounds.min.x + position.x * tileSizeX + tileSizeX / 2,
            transform.position.y + 0.01f,
            boardRenderer.bounds.min.z + position.y * tileSizeZ + tileSizeZ / 2
        );
    }

    private void InitTiles()
    {
        if (tiles.Length == 0) return;

        int index = 0;
        Vector2Int origin = new(0, 0);
        Vector2Int direction = new(0, 1);
        Vector2Int position = new(0, 0);

        float tileSize = 1f / rows;


        do
        {   
            Tile tile = tiles[index];
            Renderer tileRenderer = tile.GetComponent<Renderer>();

            // Inicializar o tile
            tile.transform.parent = transform;
            tile.transform.localScale = new(tileSize, tileSize, tileSize);
            tileRenderer.material.color = Utils.GetRandomColor();
            SetTilePosition(tile, position, tileRenderer);

            // Logica para percorrer somente as celulas da borda do grid
            if (IsCorner(position) && position != origin)
            {
                direction = new(direction.y, -direction.x);
            }

            position += direction;
            index++;
        } while (position != origin && index < tiles.Length);

    }
    
    void Start()
    {
        boardRenderer = GetComponent<Renderer>();
        InitTiles();
    }

    void Update()
    {
        
    }
}
