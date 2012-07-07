using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

using ExpROSE.IO;
using ExpROSE.Managers;

namespace ExpROSE.Sockets
{
    // <summary>
    /// Asynchronous socket server for the game connections.
    /// </summary>
    class Listener
    {
        private static int S_MAX_CONNECTIONS = 5000;
        private static Socket s_Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static Socket[] s_Worker = new Socket[S_MAX_CONNECTIONS];
        internal static int NewSocket;
        /// <summary> 
        /// Initializes the socket listener for game connections and starts listening. 
        /// </summary> 
        /// <param name="bindPort">The port where the socket listener should be bound to.</param> 
        /// <param name="maxConnections">The maximum amount of simultaneous connections.</param>
        internal static bool init(int bindPort, int maxConnections, bool debugmode)
        {
            Out.WriteLine("Starting up asynchronous socket server for game connections for port " + bindPort + "...");
            try
            {
                s_Listener.Bind(new IPEndPoint(IPAddress.Any, bindPort));
                s_Listener.Listen(20);
                s_Listener.BeginAccept(new AsyncCallback(connectionRequest), null);

                Out.WriteLine("Asynchronous socket server for game connections running on port " + bindPort);
                Out.WriteLine("Max simultaneous connections is " + maxConnections);
                return true;
            }
            catch (Exception ex)
            {
                if (debugmode == true)
                {
                    Out.WriteError("Error while setting up asynchronous socket server for game connections on port " + bindPort);
                    Out.WriteError("Port " + bindPort + "  could be invalid or in use already.");
                    Out.WriteBlank();
                    Out.WriteError(ex.Message, Out.logFlags.UnimportantAction);
                    Out.WriteBlank();
                }
                return false;
            }
        }
        /// <summary>
        /// Triggered when a connection request is recieved.
        /// </summary>
        /// <param name="syncc"></param>
        private static void connectionRequest(IAsyncResult syncc)
        {
            NewSocket = FindEmptySocket();
            if (NewSocket == 0)
            {
                Out.WriteError("Connection Rejected - no free sockets");
                return;
            }
            SocketManager.activeSockets.Add(NewSocket, s_Worker[NewSocket]);
            s_Worker[NewSocket] = s_Listener.EndAccept(syncc);
            //try
            //{


            //Character.Manager nUser = new Alpha.Character.Manager(s_Worker[NewSocket], NewSocket);
            Out.WriteLine("Accepted connection [" + NewSocket + "]");
            // Relisten for a connection
            s_Listener.BeginAccept(new AsyncCallback(connectionRequest), null);
            //nUser = null;


            //}
            /*catch (Exception ex)
            {
                if (Settings.S_DEBUG_MODE == true)
                    Out.WriteError(ex.Message);
            }*/
        }
        public static void Disconnect()
        {
            Out.WriteLine("Removing Sockets from SocketManager...");
            for (int x = 1; x < S_MAX_CONNECTIONS; x++)
            {
                if (SocketManager.activeSockets.ContainsKey(x))
                {
                    SocketManager.activeSockets.Remove(x);
                    Out.WriteLine("Removed socket: " + x + " from SocketManager.", Out.logFlags.UnimportantAction, false);
                }
            }
            Out.WriteLine("Removed from SocketManager.");
            Out.WriteLine("Closing socket listener...");
            s_Listener.Close();
            s_Worker = null;
            Out.WriteLine("Socket listener successfully closed.");
        }
        /// <summary>
        /// Finds a empty socket. if there isn't any then it will return 0
        /// </summary>
        /// <returns>The socket that is empty</returns>
        private static int FindEmptySocket()
        {
            for (int x = 1; x < S_MAX_CONNECTIONS; x++)
            {
                if (!SocketManager.activeSockets.ContainsKey(x))
                {
                    return x;
                }
            }
            return 0;
        }
    }
}
