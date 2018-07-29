using DeusClientCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeusClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start("127.0.0.1", 27015);
            bool wantToCancel = false;

            Stopwatch chrono = new Stopwatch();
            chrono.Start();

            while(!wantToCancel)
            {
                long dt = chrono.ElapsedMilliseconds;
                game.Update(dt);
                Thread.Sleep(1);
            }

            game.Stop();
        }


    }
}
