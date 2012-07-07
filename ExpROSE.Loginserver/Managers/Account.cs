using System;
using System.Collections;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using ExpROSE.IO;

namespace ExpROSE.Loginserver.Managers
{
    internal class Account
    {
        internal static int CycleValue = 0;

        #region User Details

        internal UserManager.LevelEnumerator _Access = UserManager.LevelEnumerator.Banned;
        internal string _Username = "";
        internal bool Afk = false;
        internal string Password = "";
        internal int PlayerID = 0;
        internal int state = 1;
        #endregion User Details

        internal AsyncCallback _pfnWorkerCallBack;
        internal Thread _pingThread;
        internal DateTime _timeLastPacket;
        internal bool classDroped = false;
        internal int SocketID = 0;
        private byte[] _dataBuffer = new byte[8096];
        private Socket connectionSocket;
        private string PacketBuffer = "";
        private int PacketLenght = 0;
        private bool sending = false;
        private Queue sendQueue = new Queue();


        /// <summary>
        /// Initialize new user sockets.
        /// </summary>
        /// <param name="_connectionSocket">The connection socket</param>
        /// <param name="_connectionSocketID">The connection socket id</param>
        public Account(Socket _connectionSocket, int _connectionSocketID)
        {
            try
            {
                _timeLastPacket = DateTime.Now;
                Managers.UserManager.activeSockets.Add(_connectionSocketID, this);
                this.connectionSocket = _connectionSocket;
                this.SocketID = _connectionSocketID;

                _pfnWorkerCallBack = new AsyncCallback(DataRecieved);
                WaitForData();
                /// <todo>Check if IP is banned.</todo>
            }
            catch (Exception ex)
            {
                Out.WriteError(ex.Message);
            }
        }

        ~Account()
        {
        }

        /// <summary>
        /// Completes the transfer.
        /// </summary>
        /// <param name="syncc"></param>
        internal void completeTransfer(IAsyncResult syncc)
        {
            try
            {
                if (sendQueue.Count > 0 && sending == false)
                {
                    string Data = sendQueue.Dequeue().ToString();
                    byte[] byData = Encoding.Default.GetBytes(Data + Convert.ToChar(0));
                    connectionSocket.BeginSend(byData, 0, byData.Length, 0, new AsyncCallback(completeTransfer), null);
                    sending = true;
                    byData = null;
                    Data = null;
                }
                else
                {
                    connectionSocket.EndSend(syncc);
                    sending = false;

                    if (sendQueue.Count > 0 && sending == false)
                    {
                        string Data = sendQueue.Dequeue().ToString();
                        byte[] byData = Encoding.Default.GetBytes(Data + Convert.ToChar(0));
                        connectionSocket.BeginSend(byData, 0, byData.Length, 0, new AsyncCallback(completeTransfer), null);

                        sending = true;
                        byData = null;
                        Data = null;
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Returns a string that contains the ip of the client.
        /// </summary>
        internal string connectionRemoteIP()
        {
            return connectionSocket.RemoteEndPoint.ToString().Split(':')[0];
        }

        /// <summary>
        /// Drop the connection and remove all instances from our managers.
        /// </summary>
        internal void DropConnection()
        {
            try
            {
                if (classDroped == false)
                {
                    Out.WriteLine("Dropped connection [" + _Username + "] [" + SocketID + "]", Out.logFlags.StandardAction, false);
                    connectionSocket.Close();
                }
            }
            catch { }
        }

        /// <summary>
        /// Drop the connection and remove all instances from our managers.
        /// </summary>
        /// <param name="ThrowError"></param>
        internal void DropConnection(bool ThrowError)
        {
            try
            {
                if (ThrowError == true)
                {
                    DropConnection();
                    return;
                }
                if (classDroped == false)
                {
                    Out.WriteLine("Dropped connection [" + _Username + "] [" + SocketID + "]", Out.logFlags.StandardAction, false);
                    connectionSocket.Close();
                }
                classDroped = true;
            }
            catch { }
        }

        internal bool  transferData(Packet pack)
        {
            connectionSocket.Send(pack.PacketBuffer, pack.Size, SocketFlags.None);
            return true;

        }
        /// <summary>
        /// Transfer data to the client.
        /// </summary>
        /// <param name="Data">The data that should be transfered.</param>
        internal void transferData(byte[] Data)
        {
            if (Data == null)
                return;
            try
            {
                if (connectionSocket.Connected == true)
                {
                    sendQueue.Enqueue(Data);
                    if (sendQueue.Count == 1 && sending == false)
                    {
                        byte[] byData = ExpROSE.Loginserver.Core.Main.crypt.Encrypt(Data);
                        connectionSocket.BeginSend(byData, 0, byData.Length, 0, new AsyncCallback(completeTransfer), null);
                        sending = true;
                        byData = null;
                        Data = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Out.WriteError(ex.Message);
            }
        }

        internal void transferData(string Data, bool LogIt)
        {
            if (Data == null)
                return;
            try
            {
                if (connectionSocket.Connected == true)
                {
                    sendQueue.Enqueue(Data);
                    if (sendQueue.Count == 1 && sending == false)
                    {
                        byte[] byData = Encoding.Default.GetBytes(Data + Convert.ToChar(0));
                        connectionSocket.BeginSend(byData, 0, byData.Length, 0, new AsyncCallback(completeTransfer), null);

                        sending = true;
                        byData = null;
                        Data = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Out.WriteError(ex.Message);
            }
        }

        /// <summary>
        /// This void is triggered when a new datapacket arrives at the socket of this user. On errors, the client is disconnected.
        /// </summary>
        /// <WARNING>This is some advanced stuff. Please do not mess this up.</WARNING>
        /// <param name="syncc"></param>
        private void DataRecieved(IAsyncResult syncc)
        {
            //try
            //{
            int byteRecieved = connectionSocket.EndReceive(syncc);

            if (_dataBuffer.Length < 6)
            {
                WaitForData();
                return;
            }

            byte[] packetBuffer = ExpROSE.Loginserver.Core.Main.crypt.Decrypt(ExpROSE.Loginserver.Core.Main.crypt.DecryptHeader(_dataBuffer));
            Packet pack = new Packet();

            GCHandle pinned = GCHandle.Alloc(packetBuffer, GCHandleType.Pinned);
            pack = (Packet)Marshal.PtrToStructure(pinned.AddrOfPinnedObject(), typeof(Packet));
            pinned.Free();

            if (packetBuffer != null)
            {
            
                Out.WriteLine(String.Format("{0:X}", pack.Command), Out.logFlags.BelowStandardAction, true, ConsoleColor.Red, ConsoleColor.DarkGray, _Username + ".Recieved");

                if (pack.Command == 703)
                    Managers.PacketManager.EncryptionRequest(pack, SocketID);



            }
            WaitForData();
        }

       


        private void WaitForData()
        {
            try
            {
                if (_pfnWorkerCallBack != null)
                {
                    if (connectionSocket.Connected == true)
                    {
                        _dataBuffer = new byte[connectionSocket.Available];
                        connectionSocket.BeginReceive(_dataBuffer, 0, _dataBuffer.Length, SocketFlags.None, _pfnWorkerCallBack, null);
                    }
                    else
                    {
                        DropConnection();
                    }
                }
                else
                {
                    DropConnection();
                }
            }
            catch (SocketException ex)
            {
                Out.WriteError(ex.Message);
                DropConnection();
            }
            catch (Exception ex)
            {
                Out.WriteError(ex.Message);
                DropConnection();
            }
        }

    }
}