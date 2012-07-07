﻿using System;
using System.Collections;
using System.Net.Sockets;

namespace ExpROSE.Managers
{
    /// <summary>
    /// Socket Management to return the instance of a socket.
    /// </summary>
    public static class SocketManager
    {
        public static Hashtable activeSockets = new Hashtable();

        public static Socket GetInstance(int SocketID)
        {
            //Does the socket exist?
            if (activeSockets.ContainsKey(SocketID) == true)
            {
                //Return ref to the socket object
                return (Socket)activeSockets[SocketID];
            }
            else
            {
                //Trow a null socket exception
                throw new Exception("ID doesn't exist");
            }
        }
    }
}