using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public GameObject tilePrefab;
    public GameObject xPref, oPref;
    public GameObject boardParent;
    public TextMeshProUGUI ScoreText, ResultText;
    public Tile[] tiles;
    public int[] board = new int[9];
    public bool isAi, isX, isGameOver = true;
    public bool isAiNext = true;
    public int playerScore, aiScore = 0;

    private void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        SetUpBoard();
    }

    public void ResetBoard() {
        board = new int[9];

        GameObject[] allTiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject tile in allTiles) {
            Destroy(tile);
        }

        SetUpBoard();
    }

    public void AiMove() {
        int bestMove = MiniMax.GetBestMove(board, isX);

        if (tiles[bestMove] != null) {
            MakeMove(bestMove);
        }
        else AiMove();
    }

    private IEnumerator DelayedAiMove() {
        yield return null;
        AiMove();
    }   

    public void SetNextAi(bool isOn) {
        this.isAiNext = isOn;
    }

    private void SetUpBoard() {
        isGameOver = false;
        isX = true;
        isAi = isAiNext;
        tiles = new Tile[9];
        ResultText.text = "";
        for (int i = 0; i < 9; i++) {
            GameObject tileObj = Instantiate(tilePrefab, boardParent.transform);
            tileObj.name = "tile" + i;
            tileObj.transform.position = new Vector2(i / 3 * 4 - 4 , i % 3 * 4 - 4);
            Tile tile = tileObj.GetComponent<Tile>();
            tile.index = i;
            tile.isOccupied = false;
            tiles[i] = tile;
        }
        
        if (isAi) StartCoroutine(DelayedAiMove());
    }

    public void MakeMove(int index) {
        Tile tile = tiles[index];

        if (!tile.isOccupied) {
            tile.isOccupied = true;
            if (isX) {
                tile.X.SetActive(true);
                SpriteRenderer[] xStripes = tile.X.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer stripeRenderer in xStripes) {
                    stripeRenderer.color = Color.white; 
                }
            } else {
                tile.O.SetActive(true);
                SpriteRenderer oRender = tile.O.GetComponent<SpriteRenderer>();
                oRender.color = Color.white; 
            }

            
            board[index] = this.isX ? 1 : -1;

            List<int> moves = HelperFunction.GetMoves(board);
            
            if (HelperFunction.CheckWin(board, index) != 0) {
                Win(false);
                return;
            } else if (moves.Count <= 0) {
                Win(true);
                return;
            }

            isAi = !isAi;
            isX = !isX;
        }

        if (isAi) StartCoroutine(DelayedAiMove());
    }

    public void Win(bool isDraw) {
        isGameOver = true;
        foreach (Tile tile in tiles) {
            tile.isOccupied = true;
        }

        if (isDraw) {
            ResultText.text = "DRAW";
        }
        else if (isAi) {
            aiScore++;
            ResultText.text = "AI WINS!";
        } else {
            playerScore++;
            ResultText.text = "PLAYER WINS!";
        }
        ScoreText.text = $"Player: {playerScore} \n Ai: {aiScore}";
    }
}


