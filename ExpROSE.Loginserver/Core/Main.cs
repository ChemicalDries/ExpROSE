using System;
using System.Threading;

using ExpROSE.IO;

namespace ExpROSE.Loginserver.Core
{
    internal class Main
    {
        private static Core.Threading Threads = new Core.Threading();
        private static Thread UpdateConsoleTitleThread = new Thread(new ThreadStart(Threads.ConsoleTitleUpdate));
        private static Thread ConnectionCheckThread = new Thread(new ThreadStart(Threads.ConnectionCheck));
        public static RoseCrypt.MRoseCrypt crypt = new RoseCrypt.MRoseCrypt();

        /// <summary>
        /// Boots the loginserver emulator.
        /// </summary>
        public static void Boot()
        {
            DateTime _START = DateTime.Now;
            ThreadPool.SetMaxThreads(300, 400);

            Out.WriteLine("Creating threads...");
            UpdateConsoleTitleThread.Priority = ThreadPriority.Lowest;
            UpdateConsoleTitleThread.Start();
            ConnectionCheckThread.Priority = ThreadPriority.Lowest;
            ConnectionCheckThread.Start();

            Out.WriteLine("All threads have been created.");
            Out.WriteBlank();

            crypt.GenerateLoginTables();

            Listener.init(29000, 5000, true);
            Out.WriteBlank();

            DateTime _STOP = DateTime.Now;
            TimeSpan _TST = _STOP - _START;
            Out.WriteLine("Startup time in fixed milliseconds: " + _TST.TotalMilliseconds.ToString() + ".");

            GC.Collect();
            Out.WriteLine("Experimental ROSE Emulator ready. Status: idle");
            Out.WriteBlank();

            Out.minimumImportance = Out.logFlags.UnimportantAction;
        }
    }
}