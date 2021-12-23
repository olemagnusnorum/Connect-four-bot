using System;
namespace connect
{
    public class Game
    {

        private int agent1;
        private int agent2;
        private Agent agent3;

        private Board board;

        public Game(int agent1, int agent2, int rows, int cols, int inarow)
        {
            this.agent1 = agent1;
            this.agent2 = agent2;
            this.agent3 = new Agent(rows, cols, inarow, this.agent2);
            

            this.board = new Board(rows, cols, inarow);
        }



        public void Play()
        {
            bool gameOver = false;
            int turn = -1;
            int status;

            while (!gameOver)
            {
                turn++;

                if (turn % 2 == 0)
                {
                    Console.WriteLine("col: ");
                    string col = Console.ReadLine();
                    int c = Int32.Parse(col);
                    status = this.board.AddPiece(c-1, agent1);
                    this.board.Display();
                }
                else
                {
                    status = this.board.AddPiece(this.agent3.MiniMaxPlacePiece(this.board.board), agent2);
                    this.board.Display();
                }

                if (status != 0)
                {
                    gameOver = true;
                }

            }
        }
    }
}
