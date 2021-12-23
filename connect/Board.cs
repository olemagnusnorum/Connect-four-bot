using System;
namespace connect
{
    public class Board
    {

        private int rows;
        private int cols;
        private int inarow;
        private int emptySlots;

        public int[,] board;

        public Board(int rows, int cols, int inarow)
        {
            this.rows = rows;
            this.cols = cols;
            this.inarow = inarow;
            this.emptySlots = rows * cols;
            this.board = new int[this.rows, this.cols];
        }


        public void Display()
        {
            Console.WriteLine("   1   2   3   4   5   6   7  ");
            Console.WriteLine(" -----------------------------");
            for (int i = 0; i < this.board.GetLength(0); i++)
            {
                for (int j = 0; j < this.board.GetLength(1); j++)
                {
                    Console.Write(" | " +this.board[i,j]);
                }
                Console.WriteLine(" |");
            }
            Console.WriteLine(" -----------------------------");
        }


        public int AddPiece(int col, int piece)
        {
            if (this.CheckValidMove(col))
            {
                this.emptySlots--;
                for (int i = this.rows - 1; i >= 0; i--)
                {
                    if (this.board[i,col] == 0)
                    {
                        this.board[i, col] = piece;
                        break;
                    }
                }
                return this.CheckGameOver(piece);
            }

            return -1;
        }


        public bool CheckValidMove(int col)
        {
            if (this.board[0, col] == 0)
            {
                return true;
            }
            return false;
        }


        private int CheckGameOver(int piece)
        {
            if (this.emptySlots == 0)
            {
                return -1;
            }
            if (this.CheckVerticalWin(piece))
            {
                return piece;
            }
            if (this.CheckHorizontalWin(piece))
            {
                return piece;
            }
            if (this.CheckDiagonalRightWin(piece))
            {
                return piece;
            }
            if (this.CheckDiagonalLeftWin(piece))
            {
                return piece;
            }
            return 0;
        }


        private bool CheckVerticalWin(int piece)
        {

            for (int col = 0; col < this.cols; col++)
            {
                for (int row = 0; row < this.rows - this.inarow + 1; row++)
                {
                    int count = 0;
                    for (int offset = 0; offset < this.inarow; offset++)
                    {
                        if (this.board[row+offset, col] == piece)
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


        private bool CheckHorizontalWin(int piece)
        {

            for (int row = 0; row < this.rows; row++)
            {
                for (int col = 0; col < this.cols - this.inarow + 1; col++)
                {
                    int count = 0;
                    for (int offset = 0; offset < this.inarow; offset++)
                    {
                        if (this.board[row, col+offset] == piece)
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


        private bool CheckDiagonalRightWin(int piece)
        {
            for (int row = this.inarow - 1; row < this.rows; row++)
            {
                for (int col = 0; col < this.cols - this.inarow + 1; col++)
                {
                    int count = 0;
                    for (int offset = 0; offset < this.inarow; offset++)
                    {
                        if (this.board[row-offset, col+offset] == piece)
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


        private bool CheckDiagonalLeftWin(int piece)
        {

            for (int row = 0; row < this.rows - this.inarow + 1; row++)
            {
                for (int col = 0; col < this.cols - this.inarow + 1; col++)
                {
                    int count = 0;
                    for (int offset = 0; offset < this.inarow; offset++)
                    {
                        if (this.board[row+offset, col+offset] == piece)
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



        public void Reset()
        {
            for (int row = 0; row < this.rows; row++)
            {
                for (int col = 0; col < this.cols; col++)
                {
                    this.board[row, col] = 0;
                }
            }
        }


    }
}
