using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace LeyendsServer
{
    class GameSend
    {
        /// <summary>Sends a packet to a client via TCP.</summary>
        /// <param name="_toClient">The client to send the packet the packet to.</param>
        /// <param name="_packet">The packet to send to the client.</param>
        private static void SendTCPData(int _toRoom, Packet _packet)
        {
            _packet.WriteLength();
            Server.rooms[_toRoom].gameClient.SendData(_packet);
        }

        #region Packets
        /// <summary>Sends a welcome message to the given client.</summary>
        /// <param name="_toClient">The client to send the packet to.</param>
        /// <param name="_msg">The message to send.</param>
        public static void Welcome(int _toClient, string _msg)//ID:1
        {
            using (Packet _packet = new Packet((int)RoomPackets.welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);

                SendTCPData(_toClient, _packet);
            }
        }
        #endregion
    }
}
