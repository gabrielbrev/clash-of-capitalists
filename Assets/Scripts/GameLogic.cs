using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private static readonly WaitForSeconds _waitForSeconds0_75 = new(0.75f);
    private static readonly WaitForSeconds _waitForSeconds1_25 = new(1.25f);
    [SerializeField] private Board board;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private int numPlayers; // Temporario, deverá ser setado pelo menu futuramente
    private readonly List<Player> players = new();
    private int currRound = 0;

    private IEnumerator InitializePlayers()
    {
        yield return new WaitUntil(() => board.didStart);

        Tile startTile = board.GetTile(0);

        for (int i = 0; i < numPlayers; i++)
        {
            GameObject playerObject = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            Player player = playerObject.GetComponent<Player>();

            player.transform.parent = board.transform;
            player.MoveTo(startTile);

            players.Add(player);
        }
    }

    private IEnumerator PlayRound()
    {
        Player player = players[currRound % players.Count];

        int currIndex = player.GetIndex();
        int nextIndex = currIndex + Random.Range(1, 6);

        for (int i = currIndex + 1; i <= nextIndex; i++)
        {
            Tile tile = board.GetTile(i);

            player.MoveTo(tile);

            if (i != nextIndex)
            {
                tile.PassBy(player);
            }
            else
            {
                tile.Visit(player);
            }

            yield return _waitForSeconds0_75;
        }

        currRound += 1;
        yield return _waitForSeconds1_25;
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

    void Start()
    {
        StartCoroutine(InitializeAndStartGame());
    }


    void Update()
    {
        
    }
}
