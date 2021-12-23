using System;

namespace connect
{
    class Program
    {
        static void Main(string[] args)
        {
            Game g = new Game(1, 2, 6, 7, 4);
            g.Play();
        }
    }
}
