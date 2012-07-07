using System;
using System.Collections;

namespace ExpROSE.Loginserver.Managers
{
    internal class UserManager
    {
        public static Hashtable activeSockets = new Hashtable();
        public static Hashtable activeUserClasses = new Hashtable();

        internal enum LevelEnumerator { Banned = 0, Normal = 1, Legend = 2, OrangeVIP = 3, BlueVIP = 4, PinkVIP = 5, BlackVIP = 6, Mod = 7, Staff = 8, HeadAdmin = 9 };

        internal static Managers.Account GetInstance(int SocketID)
        {
            if (activeSockets.ContainsKey(SocketID))
            {
                return (Managers.Account)activeSockets[SocketID];
            }
            else
            {
                throw new Exception("User id doesn't exist");
            }
        }

        internal static Managers.Account GetInstance(int CharacterID, bool ofCharacter)
        {
            if (ofCharacter == false)
            {
                GetInstance(CharacterID);
                throw new Exception("");
            }
            else
            {
                if (activeUserClasses.ContainsKey(CharacterID) == true)
                {
                    return (Managers.Account)activeUserClasses[CharacterID];
                }
                else
                {
                    throw new Exception("ID doesn't exist");
                }
            }
        }

        /// <summary>
        /// Disconnect a user from the server.
        /// </summary>
        /// <param name="SocketID">The Socket ID of the user.</param>
        internal void DisconnectUser(int SocketID)
        {
            try
            {
                Managers.Account _user = GetInstance(SocketID);
                _user.DropConnection();
            }
            catch
            {
                IO.Out.WriteError("Error while disconnecting user " + SocketID);
            }
        }

        /// <summary>
        /// Disconnect all users from the server.
        /// </summary>
        internal void DisconnectAllUsers()
        {
            Hashtable tmpHashtable = (Hashtable)activeSockets.Clone();
            IEnumerator myEnum = tmpHashtable.GetEnumerator();
            int x = 0;
            while (myEnum.MoveNext())
            {
                if (x == 100)
                {
                    Console.Clear();
                    x = 0;
                }
                DictionaryEntry de = (DictionaryEntry)myEnum.Current;
                Managers.Account _user = (Managers.Account)de.Value;

                _user.DropConnection();
                x++;
            }
        }

        /// <summary>
        /// Disconnect all users with a specific access level or 'rank'
        /// </summary>
        /// <param name="Access">The access level or 'rank'</param>
        internal void DisconnectAllUsers(UserManager.LevelEnumerator Access)
        {
            Hashtable tmpHashtable = (Hashtable)activeSockets.Clone();
            IEnumerator myEnum = tmpHashtable.GetEnumerator();
            int x = 0;
            while (myEnum.MoveNext())
            {
                DictionaryEntry de = (DictionaryEntry)myEnum.Current;
                Managers.Account _user = (Managers.Account)de.Value;
                if (x == 100)
                {
                    Console.Clear();
                    x = 0;
                }
                if ((int)_user._Access == (int)Access)
                {
                    _user.DropConnection();
                    x++;
                }
            }
        }

        /// <summary>
        /// Transfer data to a specific user that is connected.
        /// </summary>
        /// <param name="Data">The data that to transfer</param>
        /// <param name="SocketID">SocketID of the connected client.</param>
        internal bool TransferToUser(byte[] Data, int SocketID)
        {
            try
            {
                Managers.Account _user = GetInstance(SocketID);
                _user.transferData(Data);
                return true;
            }
            catch
            {
                IO.Out.WriteError("User " + SocketID + " was not online when data: " + Data + " was trying to be sent.", ExpROSE.IO.Out.logFlags.UnimportantAction);
                return false;
            }
        }

        /// <summary>
        /// Transfer data to all connected users.
        /// </summary>
        /// <param name="Data">The data to transfer.</param>
        internal void TransferToAll(string Data)
        {
            Hashtable tmpHashtable = (Hashtable)activeSockets.Clone();
            IEnumerator myEnum = tmpHashtable.GetEnumerator();
            while (myEnum.MoveNext())
            {
                Managers.Account.CycleValue++;
                DictionaryEntry de = (DictionaryEntry)myEnum.Current;
                Managers.Account user = (Managers.Account)de.Value;

                user.transferData(Data, false);
            }
        }

        /// <summary>
        /// Transfer data to all connected users with a specific access level.
        /// </summary>
        /// <param name="Data">The data to transfer.</param>
        /// <param name="Access">The access level or 'rank'</param>
        internal void TransferToAll(string Data, UserManager.LevelEnumerator Access)
        {
            Hashtable tmpHashtable = (Hashtable)activeSockets.Clone();
            IEnumerator myEnum = tmpHashtable.GetEnumerator();
            while (myEnum.MoveNext())
            {
                Managers.Account.CycleValue++;
                DictionaryEntry de = (DictionaryEntry)myEnum.Current;
                Managers.Account user = (Managers.Account)de.Value;

                if ((int)user._Access == (int)Access)
                    user.transferData(Data, false);
            }
        }

        /// <summary>
        /// Transfer data to all connected users with a specific access level.
        /// </summary>
        /// <param name="Data">The data to transfer.</param>
        /// <param name="Access">The access level or 'rank'</param>
        /// <param name="IncludeAbove">If true it will send to all with the specific rank and all higher</param>
        internal void TransferToAll(string Data, UserManager.LevelEnumerator Access, bool IncludeAbove)
        {
            Hashtable tmpHashtable = (Hashtable)activeSockets.Clone();
            IEnumerator myEnum = tmpHashtable.GetEnumerator();
            while (myEnum.MoveNext())
            {
                Managers.Account.CycleValue++;
                DictionaryEntry de = (DictionaryEntry)myEnum.Current;
                Managers.Account user = (Managers.Account)de.Value;
                if (IncludeAbove == false)
                {
                    if ((int)user._Access == (int)Access)
                    {
                        user.transferData(Data, false);
                    }
                }
                else
                {
                    if ((int)user._Access >= (int)Access)
                    {
                        user.transferData(Data, false);
                    }
                }
            }
        }
    }
}