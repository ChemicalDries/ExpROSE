using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ExpROSE.IO;

namespace ExpROSE.Loginserver.Core
{
    class Main
    {
        private static Core.Threading Threads = new Core.Threading();
        private static Thread UpdateConsoleTitleThread = new Thread(new ThreadStart(Threads.ConsoleTitleUpdate));
        /// <summary>
        /// Boots the loginserver emulator.
        /// </summary>
        public static void Boot()
        {
            Out.WriteLine("Creating threads...");
            UpdateConsoleTitleThread.Priority = ThreadPriority.Lowest;
            UpdateConsoleTitleThread.Start();

            Out.WriteLine("All threads have been created.");
            Out.WriteBlank();
        }
    }
}
