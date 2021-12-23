using System;
using System.Collections.Generic;

namespace connect
{
    public class Agent
    {

        private readonly int best = 1000000000;
        private readonly int good = 100;
        private readonly int ok = 1;
        private readonly int bad = -1;
        private readonly int worse = -100;
        private readonly int worst = -1000000000;

        private int rows;
        private int cols;
        private int inarow;
        private int agentPiece;
        private int opPiece;

        private Random rnd = new Random();

        private Dictionary<(int, int, int, int), int> heuristic = new();

        private Dictionary<string, int> transTable = new();

        public Agent(int rows, int cols, int inarow, int piece)
        {
            this.rows = rows;
            this.cols = cols;
            this.inarow = inarow;
            this.agentPiece = piece;
            this.opPiece = Math.Abs(piece - 3);
            this.AddHeuristic();
        }


        private void AddHeuristic()
        {
            this.heuristic.Add((this.agentPiece, this.agentPiece, this.agentPiece, this.agentPiece), this.best);

            this.heuristic.Add((this.agentPiece, this.agentPiece, this.agentPiece, 0), this.good);
            this.heuristic.Add((this.agentPiece, this.agentPiece, 0, this.agentPiece), this.good);
            this.heuristic.Add((this.agentPiece, 0, this.agentPiece, this.agentPiece), this.good);
            this.heuristic.Add((0, this.agentPiece, this.agentPiece, this.agentPiece), this.good);

            this.heuristic.Add((this.agentPiece, this.agentPiece, 0, 0), this.ok);
            this.heuristic.Add((this.agentPiece, 0, this.agentPiece, 0), this.ok);
            this.heuristic.Add((0, this.agentPiece, this.agentPiece, 0), this.ok);
            this.heuristic.Add((this.agentPiece, 0, 0, this.agentPiece), this.ok);
            this.heuristic.Add((0, this.agentPiece, 0, this.agentPiece), this.ok);
            this.heuristic.Add((0, 0, this.agentPiece, this.agentPiece), this.ok);

            this.heuristic.Add((this.opPiece, this.opPiece, 0, 0), this.bad);
            this.heuristic.Add((this.opPiece, 0, this.opPiece, 0), this.bad);
            this.heuristic.Add((0, this.opPiece, this.opPiece, 0), this.bad);
            this.heuristic.Add((this.opPiece, 0, 0, this.opPiece), this.bad);
            this.heuristic.Add((0, this.opPiece, 0, this.opPiece), this.bad);
            this.heuristic.Add((0, 0, this.opPiece, this.opPiece), this.bad);

            this.heuristic.Add((this.opPiece, this.opPiece, this.opPiece, 0), this.worse);
            this.heuristic.Add((this.opPiece, this.opPiece, 0, this.opPiece), this.worse);
            this.heuristic.Add((this.opPiece, 0, this.opPiece, this.opPiece), this.worse);
            this.heuristic.Add((0, this.opPiece, this.opPiece, this.opPiece), this.worse);

            this.heuristic.Add((this.opPiece, this.opPiece, this.opPiece, this.opPiece), this.worst);
        }



        private List<int> GetValidMoves(int[,] board)
        {
            List<int> validMoves = new List<int>();
            for (int col = 0; col < this.cols; col++)
            {
                if (board[0,col] == 0)
                {
                    validMoves.Add(col);
                }
            }
            return validMoves;
        }


        public void PrintValidMoves(int[,] board)
        {
            for (int col = 0; col < this.cols; col++)
            {
                if (board[0,col] == 0)
                {
                    Console.Write(col + "\t");
                }
            }
        }


        public int PlacePiece(int[,] board)
        {
            List<int> validMoves = this.GetValidMoves(board);
            int index = rnd.Next(validMoves.Count);
            return validMoves[index];
        }


        private int[,] AddPiece(int[,] board, int col, int piece)
        {
            int[,] boardCopy = board.Clone() as int[,];
            for (int row = this.rows-1; row >= 0; row--)
            {
                if (boardCopy[row,col] == 0)
                {
                    boardCopy[row, col] = piece;
                    break;
                }
            }
            return boardCopy;
        }

        // Checking score

        private int CheckHorizontalScore(int[,] board)
        {
            int horizontalScore = 0;

            for (int row = 0; row < this.rows; row++)
            {
                for (int col = 0; col < this.cols - this.inarow + 1; col++)
                {
                    int[] sequence = new int[this.inarow];

                    for (int offset = 0; offset < this.inarow; offset++)
                    {
                        sequence[offset] = board[row, col + offset];
                    }

                    var key = (sequence[0], sequence[1], sequence[2], sequence[3]);
                    if (this.heuristic.ContainsKey(key))
                    {
                        horizontalScore += this.heuristic[(sequence[0], sequence[1], sequence[2], sequence[3])];
                    }
                    else
                    {
                        horizontalScore += 0;
                    }
                }
            }
            return horizontalScore;
        }


        private int CheckVertiaclScore(int[,] board)
        {
            int verticalScore = 0;

            for (int col = 0; col < this.cols; col++)
            {
                for (int row = 0; row < this.rows - this.inarow + 1; row++)
                {
                    int[] sequence = new int[this.inarow];

                    for (int offset = 0; offset < this.inarow; offset++)
                    {
                        sequence[offset] = board[row + offset, col];
                    }

                    var key = (sequence[0], sequence[1], sequence[2], sequence[3]);
                    if (this.heuristic.ContainsKey(key))
                    {
                        verticalScore += this.heuristic[(sequence[0], sequence[1], sequence[2], sequence[3])];
                    }
                    else
                    {
                        verticalScore += 0;
                    }
                }
            }
            return verticalScore;
        }


        private int CheckDiagonalRightScore(int[,] board)
        {
            int diagonalRightScore = 0;

            for (int row = this.inarow - 1; row < this.rows; row++)
            {
                for (int col = 0; col < this.cols - this.inarow + 1; col++)
                {
                    int[] sequence = new int[this.inarow];

                    for (int offset = 0; offset < this.inarow; offset++)
                    {
                        sequence[offset] = board[row - offset, col + offset];
                    }

                    var key = (sequence[0], sequence[1], sequence[2], sequence[3]);
                    if (this.heuristic.ContainsKey(key))
                    {
                        diagonalRightScore += this.heuristic[(sequence[0], sequence[1], sequence[2], sequence[3])];
                    }
                    else
                    {
                        diagonalRightScore += 0;
                    }
                }
            }
            return diagonalRightScore;
        }


        private int CheckDiagonalLeftScore(int[,] board)
        {
            int diagonalLeftScore = 0;

            for (int row = 0; row < this.rows - this.inarow + 1; row++)
            {
                for (int col = 0; col < this.cols - this.inarow + 1; col++)
                {

                    int[] sequence = new int[this.inarow];

                    for (int offset = 0; offset < this.inarow; offset++)
                    {
                        sequence[offset] = board[row + offset, col + offset];
                    }
                    var key = (sequence[0], sequence[1], sequence[2], sequence[3]);
                    if (this.heuristic.ContainsKey(key))
                    {
                        diagonalLeftScore += this.heuristic[(sequence[0], sequence[1], sequence[2], sequence[3])];
                    }
                    else
                    {
                        diagonalLeftScore += 0;
                    }
                }
            }
            return diagonalLeftScore;
        }


        private int CheckBoardStateScore(int[,] board)
        {
            return this.CheckHorizontalScore(board) + this.CheckVertiaclScore(board) + this.CheckDiagonalRightScore(board) + this.CheckDiagonalLeftScore(board);
        }


        // Checking game over


        private bool CheckVerticalWin(int[,] board, int piece)
        {
            for (int col = 0; col < this.cols; col++)
            {
                for (int row = 0; row < this.rows - this.inarow + 1; row++)
                {
                    int count = 0;
                    for (int offset = 0; offset < this.inarow; offset++)
                    {
                        if (board[row + offset, col] == piece)
                        {
                            count++;
                        }
                    }

                    if (count == this.inarow)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        private bool CheckHorizontalWin(int[,] board, int piece)
        {
            for (int row = 0; row < this.rows; row++)
            {
                for (int col = 0; col < this.cols - this.inarow + 1; col++)
                {
                    int count = 0;
                    for (int offset = 0; offset < this.inarow; offset++)
                    {
                        if (board[row, col + offset] == piece)
                        {
                            count++;
                        }
                    }
                    if (count == this.inarow)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        private bool CheckDiagonalRightWin(int[,] board, int piece)
        {
            for (int row = this.inarow - 1; row < this.rows; row++)
            {
                for (int col = 0; col < this.cols - this.inarow + 1; col++)
                {
                    int count = 0;
                    for (int offset = 0; offset < this.inarow; offset++)
                    {
                        if (board[row - offset, col + offset] == piece)
                        {
                            count++;
                        }
                    }
                    if (count == this.inarow)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        private bool CheckDiagonalLeftWin(int[,] board, int piece)
        {
            for (int row = 0; row < this.rows - this.inarow + 1; row++)
            {
                for (int col = 0; col < this.cols - this.inarow + 1; col++)
                {
                    int count = 0;
                    for (int offset = 0; offset < this.inarow; offset++)
                    {
                        if (board[row + offset, col + offset] == piece)
                        {
                            count++;
                        }
                    }
                    if (count == this.inarow)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CheckGameOver(int[,] board, int piece)
        {
            bool filled = true;
            for (int col = 0; col < this.cols; col++)
            {
                if (board[0, col] == 0)
                {
                    filled = false;
                    break;
                }
            }
            if (filled)
            {
                return true;
            }


            if (this.CheckVerticalWin(board, piece))
            {
                return true;
            }
            if (this.CheckHorizontalWin(board, piece))
            {
                return true;
            }
            if (this.CheckDiagonalRightWin(board, piece))
            {
                return true;
            }
            if (this.CheckDiagonalLeftWin(board, piece))
            {
                return true;
            }
            return false;
        }

        private string GenerateKey(int[,] board)
        {
            string key = "";
            for (int row = 0; row < this.rows; row++)
            {
                for (int col = 0; col < this.cols; col++)
                {
                    key += board[row, col].ToString();
                }
            }
            return key;
        }


        // Minimax algorithm and extra methods

        private List<(int[,], int move)> GetChildNodes(int[,] board, int piece)
        {
            List<(int[,], int)> childNodes = new List<(int[,],int)>();

            List<int> validMoves = this.GetValidMoves(board);

            for (int i = 0; i < validMoves.Count; i++)
            {
                int[,] boardCopy = this.AddPiece(board, validMoves[i], piece);
                childNodes.Add((boardCopy, validMoves[i]));
            }

            return childNodes;
        }


        public (int, int) MiniMax(int[,] board, int depth, int alpha, int beta, int piece)
        {
            int value;

            if (depth == 0)
            {
                return (this.CheckBoardStateScore(board), -1);
            }
            if (this.CheckGameOver(board, Math.Abs(piece - 3)))
            {
                return (this.CheckBoardStateScore(board), -1);
            }

            // If maximizing player
            if (piece == this.agentPiece)
            {
                int move = -1;
                int childNodeStateValue;
                int x;

                value = int.MinValue;
                List<(int[,] childBoard, int move)> childNodes = this.GetChildNodes(board, piece);

                for (int i = 0; i < childNodes.Count; i++)
                {
                    (int[,] childNode, int childMove) = childNodes[i];

                    //transTable
                    string key = this.GenerateKey(childNode);
                    if (this.transTable.ContainsKey(key))
                    {
                        childNodeStateValue = this.transTable[key];
                    }
                    else
                    {
                        (childNodeStateValue, x) = this.MiniMax(childNode, depth - 1, alpha, beta, this.opPiece);
                        this.transTable.Add(key, childNodeStateValue);
                    }


                    if (value < childNodeStateValue)
                    {
                        value = childNodeStateValue;
                        move = childMove;
                    }
                    if (value >= beta)
                    {
                        break;
                    }
                    alpha = Math.Max(alpha, value);
                }
                return (value, move);
            }


            // If minimizing player
            if (piece == this.opPiece)
            {
                int move = -1;
                int childNodeStateValue;
                int x;

                value = int.MaxValue;
                List<(int[,] childBoard, int move)> childNodes = this.GetChildNodes(board, piece);

                for (int i = 0; i < childNodes.Count; i++)
                {
                    (int[,] childNode, int childMove) = childNodes[i];

                    //transTable
                    string key = this.GenerateKey(childNode);
                    if (this.transTable.ContainsKey(key))
                    {
                        childNodeStateValue = this.transTable[key];
                    }
                    else
                    {
                        (childNodeStateValue, x) = this.MiniMax(childNode, depth - 1, alpha, beta, this.agentPiece);
                        this.transTable.Add(key, childNodeStateValue);
                    }

                    if (value > childNodeStateValue)
                    {
                        value = childNodeStateValue;
                        move = childMove;
                    }
                    if (value <= alpha)
                    {
                        break;
                    }
                    beta = Math.Min(beta, value);
                }
                return (value, move);

            }
            return (-1, -1);
        }



        public int MiniMaxPlacePiece(int[,] board)
        {
            (int value, int move) = this.MiniMax(board, 10, int.MinValue, int.MaxValue, this.agentPiece);

            return move;
        }

    }
}
