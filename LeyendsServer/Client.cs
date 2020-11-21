using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using MongoDB.Bson;

namespace LeyendsServer
{
    class Client
    {
        public static int dataBufferSize = 4096;
        public int id;
        public ObjectId token;
        public string nickName;
        public bool queueStatus;
        public string queueType;
        public int groupLeader;
        public int roomId;
        public bool inGame;
        public TCP tcp;
        public UDP udp;
        public List<FriendReference> user_friends;

        public Client(int _clientId, ObjectId _token)
        {
            id = _clientId;
            token = _token;
            nickName = null;
            queueStatus = false;
            queueType = null;
            groupLeader = 0;
            roomId = 0;
            inGame = false;
            tcp = new TCP(id, _token);
            udp = new UDP(id);

        }
        public Client()
        {
            id = 1;
            token = ObjectId.Empty;
            nickName = "GameServerClient";
            queueStatus = false;
            queueType = null;
            groupLeader = 0;
            roomId = 0;
            inGame = false;
            tcp = new TCP(1);

        }
        public FriendReference GetFriendReference()
        {
            return new FriendReference(token, id, nickName);
        }
        public List<ObjectId> GetFriendsKeys()
        {
            List<ObjectId> friendsKey = new List<ObjectId>();
            if (user_friends == null) return friendsKey;
            foreach (FriendReference item in user_friends)
            {
                friendsKey.Add(item._oid);
            }
            return friendsKey;
        }
        public override string ToString()
        {
            if (groupLeader == 0)
            {
                return "Client ID: " + id + " - Nick: " + nickName;
            }
            else
            {
                return "Client ID: " + id + " - OnGroupOf: " + Server.clients[groupLeader].nickName + " - Nick: " + nickName + " - OnQueue: " + queueStatus + " - QueueType: " + queueType + " - InGame: " + inGame;
            }

        }
        public class TCP
        {
            public TcpClient socket;
            private readonly int id;
            private readonly ObjectId token;
            private NetworkStream stream;
            private Packet receivedData;
            private byte[] receiveBuffer;

            public TCP(int _id, ObjectId _token)
            {
                id = _id;
                token = _token;
            }
            public TCP(int _id)
            {
                id = _id;
                token = ObjectId.Empty;
            }
            /// <summary>Initializes the newly connected client's TCP-related info.</summary>
            /// <param name="_socket">The TcpClient instance of the newly connected client.</param>
            public void Connect(TcpClient _socket)
            {
                socket = _socket;
                socket.ReceiveBufferSize = dataBufferSize;
                socket.SendBufferSize = dataBufferSize;

                stream = socket.GetStream();

                receivedData = new Packet();
                receiveBuffer = new byte[dataBufferSize];

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                ServerSend.Welcome(id, "Welcome to Auth Server");
            }

            /// <summary>Sends data to the client via TCP.</summary>
            /// <param name="_packet">The packet to send.</param>
            public void SendData(Packet _packet)
            {
                try
                {
                    if (socket != null)
                    {
                        stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null); // Send data to appropriate client
                    }
                }
                catch (Exception _ex)
                {
                    Console.WriteLine($"Error sending data to player {id} via TCP: {_ex}");
                }
            }

            /// <summary>Reads incoming data from the stream.</summary>
            private void ReceiveCallback(IAsyncResult _result)
            {
                try
                {
                    int _byteLength = stream.EndRead(_result);
                    if (_byteLength <= 0)
                    {
                        Server.clients[id].Disconnect();
                        return;
                    }

                    byte[] _data = new byte[_byteLength];
                    Array.Copy(receiveBuffer, _data, _byteLength);

                    receivedData.Reset(HandleData(_data)); // Reset receivedData if all data was handled
                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                }
                catch (Exception _ex)
                {
                    Console.WriteLine($"Error receiving TCP data: {_ex}");
                    Server.clients[id].Disconnect();
                }
            }

            /// <summary>Prepares received data to be used by the appropriate packet handler methods.</summary>
            /// <param name="_data">The recieved data.</param>
            private bool HandleData(byte[] _data)
            {
                int _packetLength = 0;

                receivedData.SetBytes(_data);

                if (receivedData.UnreadLength() >= 4)
                {
                    // If client's received data contains a packet
                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        // If packet contains no data
                        return true; // Reset receivedData instance to allow it to be reused
                    }
                }

                while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
                {
                    // While packet contains data AND packet data length doesn't exceed the length of the packet we're reading
                    byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                    ThreadManager.ExecuteOnMainThread(() =>
                    {
                        using (Packet _packet = new Packet(_packetBytes))
                        {
                            int _packetId = _packet.ReadInt();
                            try
                            {
                                Server.fromClientpacketHandlers[_packetId](id, _packet); // Call appropriate method to handle the packet 
                            }
                            catch (System.Exception)
                            {
                                Console.WriteLine($"Unhandled MESSAGE ID ERROR {_packetId}");
                                ServerSend.SendTrash(id, (int)ErrorCode.General);
                                throw;
                            }

                        }
                    });

                    _packetLength = 0; // Reset packet length
                    if (receivedData.UnreadLength() >= 4)
                    {
                        // If client's received data contains another packet
                        _packetLength = receivedData.ReadInt();
                        if (_packetLength <= 0)
                        {
                            // If packet contains no data
                            return true; // Reset receivedData instance to allow it to be reused
                        }
                    }
                }

                if (_packetLength <= 1)
                {
                    return true; // Reset receivedData instance to allow it to be reused
                }

                return false;
            }

            /// <summary>Closes and cleans up the TCP connection.</summary>
            public void Disconnect()
            {
                socket.Close();
                stream = null;
                receivedData = null;
                receiveBuffer = null;
                socket = null;
            }
        }

        public class UDP
        {
            public IPEndPoint endPoint;

            private int id;

            public UDP(int _id)
            {
                id = _id;
            }

            /// <summary>Initializes the newly connected client's UDP-related info.</summary>
            /// <param name="_endPoint">The IPEndPoint instance of the newly connected client.</param>
            public void Connect(IPEndPoint _endPoint)
            {
                endPoint = _endPoint;
            }

            /// <summary>Sends data to the client via UDP.</summary>
            /// <param name="_packet">The packet to send.</param>
            public void SendData(Packet _packet)
            {
                Server.SendUDPData(endPoint, _packet);
            }

            /// <summary>Prepares received data to be used by the appropriate packet handler methods.</summary>
            /// <param name="_packetData">The packet containing the recieved data.</param>
            public void HandleData(Packet _packetData)
            {
                int _packetLength = _packetData.ReadInt();
                byte[] _packetBytes = _packetData.ReadBytes(_packetLength);

                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        int _packetId = _packet.ReadInt();
                        Server.fromClientpacketHandlers[_packetId](id, _packet); // Call appropriate method to handle the packet
                    }
                });
            }

            /// <summary>Cleans up the UDP connection.</summary>
            public void Disconnect()
            {
                endPoint = null;
            }
        }

        /// <summary>Disconnects the client and stops all network traffic.</summary>
        private void Disconnect()
        {
            Console.WriteLine($"{nickName} has disconnected, {tcp.socket.Client.RemoteEndPoint} ");
            DbManager.UpdateState(token, (Int32)Status.Offline, (Int32)Status.Offline);
            ServerSend.UpdateFriendStatus(id, Server.clients[id].token.ToString(), false);
            if (queueStatus)
            {
                ServerHandle.QuitQueueRequest(id, null);
            }
            if (groupLeader != 0)
            {
                if (groupLeader == id)
                {
                    Console.WriteLine($"Player {id} was group lead.");
                    ServerSend.GroupDisolved(id, true);
                }
                else
                {
                    Console.WriteLine($"Player {id} was in group.");
                    try
                    {
                        foreach (PlayerQueue item in QueueManager.preMadeGroups[groupLeader].groupMembers)
                        {
                            if (item.id == id)
                            {
                                QueueManager.preMadeGroups[groupLeader].groupMembers.Remove(item);
                                ServerSend.GroupInvitedResponse(groupLeader, id);
                                break;
                            }
                        }
                    }
                    catch (System.Exception)
                    {
                        Console.WriteLine("Grupo ya disuelto");
                    }

                }
                groupLeader = 0;
            }

            ResetSlotPlayer();
            tcp.Disconnect();
            udp.Disconnect();

        }

        private void ResetSlotPlayer()
        {
            this.token = ObjectId.Empty;
            this.nickName = null;
            this.queueType = null;
            this.groupLeader = 0;
            this.queueStatus = false;
            this.roomId = 0;
            this.inGame = false;
        }
    }
}
