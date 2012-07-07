using System;
using ExpROSE.IO;

namespace ExpROSE.Loginserver.Managers
{
    internal class PacketManager
    {
        /// <summary>
        /// Handle a EncryptionRequest that have been received from the client.
        /// </summary>
        /// <param name="pData">The packet.</param>
        /// <param name="SocketID">The Socket ID of the connected client.</param>
        internal static void EncryptionRequest(Packet pack, int SocketID)
        {

                Managers.Account _user = UserManager.GetInstance(SocketID);
                _user._timeLastPacket = DateTime.Now;

                var Packet = new Packet(0x07FF,11,00);
                Packet.AddByte(0x02);
                Packet.AddDword(0x87654321);
                Packet.EndPacket();
    
                _user.transferData(Packet);
           
        }
    }
}