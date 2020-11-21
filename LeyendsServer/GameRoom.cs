using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace LeyendsServer
{
    public class GameRoom
    {
        public int id { get; set; }
        public bool isSet { get; set; }
        public int port { get; set; }
        public int groupSize { get; set; }
        public int playersInGroup { get; set; }
        public List<int> groupsInRoom { get; set; }
        private TcpListener tcpListener;
        public RoomTCP gameClient;
        public GameRoom(int _id, int _port)
        {
            id = _id;
            isSet = false;
            port = _port;
            groupSize = 0;
            playersInGroup = 0;
            gameClient = new RoomTCP();
            groupsInRoom = new List<int>();
            Console.WriteLine($"GameRoom {id} Init");
        }
        public void Start(){
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
            Console.WriteLine($"Server listen game on port {port}.");
        }
        private void TCPConnectCallback(IAsyncResult _result)
        {
            TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
            tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
            Console.WriteLine($"Incoming connection from room {_client.Client.RemoteEndPoint} ...");

            
            gameClient.Connect(_client);

            Console.WriteLine($"{_client.Client.RemoteEndPoint} failed to connect: Server full!");
        }

        public override string ToString()
        {
            return $" [{id}] [{port}] groupSize[{groupSize}]";
        }
        public void SetGroupMember(int _groupId)
        {
            playersInGroup += QueueManager.randomQueuesGrup[_groupId].GroupSize();
            groupsInRoom.Add(QueueManager.randomQueuesGrup[_groupId].id);
            foreach (PlayerQueue item in QueueManager.randomQueuesGrup[_groupId].groupMembers)
            {
                Server.clients[item.id].roomId = id;
            }
            QueueManager.randomQueuesGrup[_groupId].isOnRoom = true;
        }
        public void RemoveGroupMember(int _groupId)
        {
            playersInGroup -= QueueManager.randomQueuesGrup[_groupId].GroupSize();
            groupsInRoom.Remove(QueueManager.randomQueuesGrup[_groupId].id);
            foreach (PlayerQueue item in QueueManager.randomQueuesGrup[_groupId].groupMembers)
            {
                Server.clients[item.id].roomId = 0;
            }
            QueueManager.randomQueuesGrup[_groupId].isOnRoom = false;
        }
        public int GroupSize()
        {
            int counter = 0;
            foreach (int item in groupsInRoom)
            {
                counter += QueueManager.randomQueuesGrup[item].GroupSize();
            }
            return counter;
        }

        public bool isReadyToCall()
        {
            return playersInGroup == groupSize;
        }

        public List<int> GroupMembers()
        {
            List<int> _members = new List<int>();
            foreach (int group in groupsInRoom)
            {
                foreach (PlayerQueue player in QueueManager.randomQueuesGrup[group].groupMembers)
                {
                    _members.Add(player.id);
                }
            }
            return _members;
        }
        public bool isGameRoomReady()
        {
            List<int> _members = new List<int>();
            foreach (int group in groupsInRoom)
            {
                foreach (PlayerQueue player in QueueManager.randomQueuesGrup[group].groupMembers)
                {
                    if (!player.queueAcepted)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public void AceptGame(int _fromPlayer)
        {
            List<int> _members = new List<int>();
            foreach (int group in groupsInRoom)
            {
                foreach (PlayerQueue player in QueueManager.randomQueuesGrup[group].groupMembers)
                {
                    if (player.id == _fromPlayer)
                    {
                        player.queueAcepted = true;
                        return;
                    }
                }
            }
        }
        public void ResetGame()
        {
            List<int> _removeGroups = new List<int>();
            List<int> _quitQueuePlayers = new List<int>();
            foreach (int group in groupsInRoom)
            {
                Console.WriteLine($"Reseting Group {group}");
                foreach (PlayerQueue player in QueueManager.randomQueuesGrup[group].groupMembers)
                {
                    if (!player.queueAcepted)
                    {
                        _quitQueuePlayers.Add(player.id);
                        Console.WriteLine($"player {Server.clients[player.id].nickName} rejects queue");
                        _removeGroups.Add(Server.clients[player.id].groupLeader);
                        break;
                    }
                }
            }
            foreach (int item in _removeGroups)
            {
                groupsInRoom.Remove(item);
            }
            foreach (int item in _quitQueuePlayers)
            {
                ServerHandle.QuitQueueRequest(item, null);
            }
            foreach (int group in groupsInRoom)
            {
                foreach (PlayerQueue player in QueueManager.randomQueuesGrup[group].groupMembers)
                {
                    player.queueAcepted = false;
                }
            }
        }
        public void SendGame()
        {
            ServerSend.GameCall(id);
        }
        public class RoomTCP
        {
            public static int dataBufferSize = 4096;
            public TcpClient socket;
            private readonly int id;
            private NetworkStream stream;
            private Packet receivedData;
            private byte[] receiveBuffer;

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
                //ServerSend.Welcome(id, "Welcome to Auth Server");

                receivedData = new Packet();
                receiveBuffer = new byte[dataBufferSize];

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                GameSend.Welcome(1, "Welcome to Auth Server");
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
                        Disconnect();
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
                    Disconnect();
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
                                Server.fromGamepacketHandlers[_packetId](id, _packet); // Call appropriate method to handle the packet 
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
    }

}

