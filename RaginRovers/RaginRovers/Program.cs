using System;
using System.Threading;

namespace RaginRovers
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0] == "server")
                {
                    return;
                }
            }

            //Create Thread
            LidgrenWorkThread workerObject = new LidgrenWorkThread();
            Thread workerThread = new Thread(workerObject.SyncComps);
            //Start Thread
            workerThread.Start();

            #region use elsewhere
            //Loop until worker thread active
            //while (!workerThread.IsAlive) ;

            //put the main thread to sleep to allow worker thread to do some work
            //Thread.Sleep(1);

            //stop worker thread
            //workerObject.RequestStop();

            //use join method
            //workerThread.Join();
            #endregion

            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}

