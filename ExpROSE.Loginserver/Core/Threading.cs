using System;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using ExpROSE.Managers;
using ExpROSE.IO;

namespace ExpROSE.Loginserver.Core
{
    internal class Threading
    {
        /// <summary>
        /// Updates the console title with memory usage.
        /// </summary>
        internal void ConsoleTitleUpdate()
        {
            while (true)
            {
                Thread.Sleep(30000);
                Console.Title = "Experimental ROSE Online Emulator - Build: " + " 1 " + " | Memory Usage: " + GC.GetTotalMemory(true) / 1024 + "KB]";
            }
        }

        internal void ConnectionCheck()
        {
            Out.WriteLine("Checking for timeout users.", Out.logFlags.UnimportantAction,false);
            Thread.Sleep(10 * 60000);
            Hashtable tmpHashtable = (Hashtable)Managers.UserManager.activeSockets.Clone();
            IEnumerator myEnum = tmpHashtable.GetEnumerator();
            while (myEnum.MoveNext())
            {
                Managers.Account.CycleValue++;
                DictionaryEntry de = (DictionaryEntry)myEnum.Current;
                Managers.Account user = (Managers.Account)de.Value;
                DateTime dtWhenItRunsOut = user._timeLastPacket.AddSeconds(1 * 60);
                if (DateTime.Now.CompareTo(dtWhenItRunsOut) == 1)
                {
                    Out.WriteLine("User on Socket ID: [" + user.SocketID + "] timed out.");
                    try
                    {
                        user.DropConnection(false);
                    }
                    catch { }
                }
            }
            tmpHashtable = null;
            GC.Collect();
        }


    }
}