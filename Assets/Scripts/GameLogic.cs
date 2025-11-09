using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private static readonly WaitForSeconds _waitForSeconds1 = new(1f);
    private static readonly WaitForSeconds _waitForSeconds3 = new(3f);
    private static readonly WaitForSeconds _movementDelay = new(0.5f);
    [SerializeField] private Board board;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerAiPrefab;
    [SerializeField] private int numPlayers; // Temporario, deverá ser setado pelo menu futuramente
    [SerializeField] private int numAiPlayers;
    private readonly List<Player> players = new();
    private int currRound = 0;
    

    private IEnumerator InitializePlayers()
    {
        yield return new WaitUntil(() => board.didStart);

        Tile startTile = board.GetTile(0);

        for (int i = 0; i < numPlayers + numAiPlayers; i++)
        {
            bool isAI = i >= numPlayers;
            GameObject playerObject = Instantiate(isAI ? playerAiPrefab : playerPrefab, Vector3.zero, Quaternion.identity);
            Player player = playerObject.GetComponent<Player>();

            player.name = $"Jogador {i + 1}{(isAI ? " (AI)" : "")}";
            player.transform.parent = board.transform;
            player.MoveTo(startTile);
            player.AddBalance(1_500_000);

            players.Add(player);
        }

        yield return _waitForSeconds3;
    }

    private IEnumerator PlayRound()
    {
        Player player = players[currRound % players.Count];

        uiManager.SetPlayerPanel(player.GetPanel());

        int diceResult = 0;
        bool repeatPlayer = false;
        yield return player.OptRollDice((result) => (diceResult, repeatPlayer) = result);

        int currIndex = player.GetIndex();
        int nextIndex = currIndex + diceResult;

        for (int i = currIndex + 1; i <= nextIndex; i++)
        {
            Tile tile = board.GetTile(i);

            player.MoveTo(tile);

            if (i != nextIndex)
            {
                yield return tile.PassBy(player);
            }
            else
            {
                yield return tile.Visit(player);
            }

            yield return _movementDelay;
        }

        if (!repeatPlayer) currRound += 1;

        yield return _waitForSeconds1;
    }

    private IEnumerator StartGame()
    {
        while (true)
        {
            yield return StartCoroutine(PlayRound());
        }
    }

    private IEnumerator InitializeAndStartGame()
    {
        yield return StartCoroutine(InitializePlayers());
        yield return StartCoroutine(StartGame());
    }

    void Awake()
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("pt-BR");
    }

    void Start()
    {
        StartCoroutine(InitializeAndStartGame());
    }


    void Update()
    {
        
    }
}
