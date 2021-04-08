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
        public static void Welcome(int _toRoom, string _msg)//ID:1
        {
            using (Packet _packet = new Packet((int)ToGamePackets.welcome))
            {
                Guid guid = Guid.NewGuid();
                Server.rooms[_toRoom].guid = guid;
                string strGuid = guid.ToString();
                _packet.Write(strGuid);
                _packet.Write(_toRoom);

                SendTCPData(_toRoom, _packet);
            }
        }

        public static void BaseMap(int _toRoom)//ID:2
        {
            using (Packet _packet = new Packet((int)ToGamePackets.baseMap))
            {
                _packet.Write(Procedural.GetTriangles());
                _packet.Write(Procedural.GetVertices());
                _packet.Write(_toRoom);

                SendTCPData(_toRoom, _packet);
            }
        }
        #endregion
    }
}
