using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;

using ExpROSE.IO;

namespace ExpROSE.Loginserver.Core
{
    class Threading
    {
        /// <summary>
        /// Updates the console title with memory usage.
        /// </summary>
        internal void ConsoleTitleUpdate()
        {
            while (true)
            {
                Thread.Sleep(30000);
                Console.Title = "AlphaWorlds Emulator Core 3 - Build: " + " 1 " + " | Memory Usage: " + GC.GetTotalMemory(true) / 1024 + "KB]";
            }
        }
    }
}
