using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    [SerializeField] private GameObject panelPrefab;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int balance;
    private PlayerPanel panel;
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

    private void UpdatePanel()
    {
        panel.SetInfo(
            $"{name}\n$ {balance},00\nPosição {index}"
        );
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

    public void AddBalance(int amount)
    {
        balance += amount;
    }

    public void SubtractBalance(int amount)
    {
        // TODO: Adicionar regra para vender propriedade ou falir ao tentar subtrair um valor maior do que o saldo
        balance -= amount;
    }

    public PlayerPanel GetPanel()
    {
        return panel;
    }

    public IEnumerator OptRollDice(System.Action<int> callback)
    {
        bool clicked = false;
        int result = 0;

        panel.SetRollButtonAction(() =>
        {
            result = Random.Range(1, 7);
            clicked = true;
        });

        panel.SetRoolButtonInteractable(true);

        yield return new WaitUntil(() => clicked);

        panel.SetRoolButtonInteractable(false);
        StartCoroutine(panel.SetDiceResult(result));

        callback.Invoke(result);
    }

    void Awake()
    {
        playerCollider = GetComponent<CapsuleCollider>();
        playerRenderer = GetComponent<Renderer>();

        GameObject panelObj = Instantiate(panelPrefab);
        panel = panelObj.GetComponent<PlayerPanel>();
    }

    void Start()
    {
        playerRenderer.material.color = Random.ColorHSV();
    }

    void Update()
    {
        UpdatePosition();
        UpdatePanel();
    }
}
