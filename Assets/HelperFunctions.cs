using UnityEngine;
using System.Collections.Generic;

public class HelperFunctions : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public static class MiniMax {
    public static int GetBestMove(int[] board, bool isX) {
         List<int> posMoves = HelperFunction.GetMoves(board);
        int bestScore = -9999;
        int bestMove = 4;

        HelperFunction.RandomizeList(posMoves);

        foreach (int move in posMoves) {
            HelperFunction.CommitMove(board, move, isX);

            int score;
            if (HelperFunction.CheckWin(board, move) != 0) {
                HelperFunction.UndoMove(board, move);
                return move;
            } else {
                score = Beta(board, !isX, 0);
            }

            if (score > bestScore) {
                bestScore = score;
                bestMove = move;
            }

            HelperFunction.UndoMove(board, move);
        }

        return bestMove;
    } 

    static int Alpha(int[] board, bool isX, int depth) {
        List<int> posMoves = HelperFunction.GetMoves(board);
        if (posMoves.Count == 0) return 0;
        int bestScore = -9999;

        foreach (int move in posMoves) {
            HelperFunction.CommitMove(board, move, isX);

            int score;
            if (HelperFunction.CheckWin(board, move) != 0) {
                score = 10 - depth;
            } else {
                score = Beta(board, !isX, depth + 1);
            }

            if (bestScore < score) {
                bestScore = score;
            }

            HelperFunction.UndoMove(board, move);
        }

        return bestScore;

    }

    static int Beta(int[] board, bool isX, int depth) {
        List<int> posMoves = HelperFunction.GetMoves(board);
        if (posMoves.Count == 0) return 0;
        int bestScore = 9999;

        foreach (int move in posMoves) {
            HelperFunction.CommitMove(board, move, isX);

            int score;
            if (HelperFunction.CheckWin(board, move) != 0) {
                score = -10 + depth;
            } else {
                score = Alpha(board, !isX, depth + 1);
            }

            if (bestScore > score) {
                bestScore = score;
            }

            HelperFunction.UndoMove(board, move);
        }

        return bestScore;

    }
}

public static class HelperFunction {
    public static void RandomizeList(List<int> list) {
        System.Random random = new System.Random();
        for (int i = 0; i < list.Count; i++) {
            int j = random.Next(0, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
            
        }
    }

    public static int CheckWin(int[] board, int lastMove) {
        int player = board[lastMove]; // GET CURRENT PLAYER EITHER 1 or -1

        int row = lastMove / 3;
        int col = lastMove % 3;

        // check row 
        if (board[row * 3] == player && board[row * 3 + 1] == player && board[row * 3 + 2] == player)  {
            return player;
        }

        // check col 
        if (board[col] == player && board[col + 3] == player && board[col + 6] == player) {
            return player;
        } 

        if (lastMove % 2 == 0 && board[4] == player) {
            if (board[0] == player && board[8] == player) return player;
            if (board[2] == player && board[6] == player) return player;
        }

        return 0;
    }

        public static List<int> GetMoves(int[] board) {
        List<int> moves = new List<int>();
        for (int i = 0 ; i < board.Length; i++) {
            if (board[i] == 0) {
                moves.Add(i);
            }
        }
        return moves;
    }

    public static void CommitMove(int[] board, int move, bool playerBool) {
        int player = playerBool ? 1 : -1;

        board[move] = player;
    }

    public static void UndoMove(int[] board, int move) {
        board[move] = 0;
    }
}
