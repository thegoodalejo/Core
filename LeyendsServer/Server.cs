using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using MongoDB.Bson;

namespace LeyendsServer
{
    class Server
    {
        public static int MaxPlayers { get; private set; }
        public static int MaxRooms { get; private set; }
        public static int Port { get; private set; }
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
        public static Dictionary<int, GameRoom> rooms = new Dictionary<int, GameRoom>();
        public delegate void PacketHandler(int _fromClient, Packet _packet);
        public static Dictionary<int, PacketHandler> fromClientpacketHandlers;
        public static Dictionary<int, PacketHandler> fromGamepacketHandlers;
        private static TcpListener tcpListener;
        private static UdpClient udpListener;
        private static List<EndPoint> registredIPS;

        /// <summary>Starts the server.</summary>
        /// <param name="_maxPlayers">The maximum players that can be connected simultaneously.</param>
        /// <param name="_port">The port to start the server on.</param>
        public static void Start(int _maxPlayers, int _maxRooms, int _port)
        {
            MaxPlayers = _maxPlayers;
            MaxRooms = _maxRooms;
            Port = _port;

            Console.WriteLine("Starting server...");
            InitializeServerData();
            InitializeRoomData();

            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);

            udpListener = new UdpClient(Port);
            udpListener.BeginReceive(UDPReceiveCallback, null);

            registredIPS = new List<EndPoint>();

            Console.WriteLine($"Server listen clients on port {Port}.");

            DbManager.UpdateStateAll();
        }
        public static Client SearchMemberByNick(string _nick)
        {
            foreach (Client item in clients.Values)
            {
                if (item.nickName == _nick)
                {
                    return item;
                }
            }
            return null;
        }
        /// <summary>Handles new TCP connections.</summary>
        private static void TCPConnectCallback(IAsyncResult _result)
        {
            TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
            tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
            Console.WriteLine($"Incoming connection from {_client.Client.RemoteEndPoint} ...");

            //TODO: despues del portforward hay q mirar como bloquear el multi account desde una misma ip, 
            //este Console.WriteLine(((System.Net.IPEndPoint)_client.Client.RemoteEndPoint).Address); 
            //retorna la IP entonces seria mas como mirar si desde el mismo cliente se podria iniciar doble cuenta 

            for (int i = 1; i <= MaxPlayers; i++)
            {
                if (clients[i].tcp.socket == null)
                {
                    registredIPS.Add(_client.Client.RemoteEndPoint);
                    clients[i].tcp.Connect(_client);
                    return;
                }
            }

            Console.WriteLine($"{_client.Client.RemoteEndPoint} failed to connect: Server full!");
        }

        /// <summary>Receives incoming UDP data.</summary>
        private static void UDPReceiveCallback(IAsyncResult _result)
        {
            try
            {
                IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] _data = udpListener.EndReceive(_result, ref _clientEndPoint);
                udpListener.BeginReceive(UDPReceiveCallback, null);

                if (_data.Length < 4)
                {
                    return;
                }

                using (Packet _packet = new Packet(_data))
                {
                    int _clientId = _packet.ReadInt();

                    if (_clientId == 0)
                    {
                        return;
                    }

                    if (clients[_clientId].udp.endPoint == null)
                    {
                        // If this is a new connection
                        clients[_clientId].udp.Connect(_clientEndPoint);
                        return;
                    }

                    if (clients[_clientId].udp.endPoint.ToString() == _clientEndPoint.ToString())
                    {
                        // Ensures that the client is not being impersonated by another by sending a false clientID
                        clients[_clientId].udp.HandleData(_packet);
                    }
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error receiving UDP data: {_ex}");
            }
        }

        /// <summary>Sends a packet to the specified endpoint via UDP.</summary>
        /// <param name="_clientEndPoint">The endpoint to send the packet to.</param>
        /// <param name="_packet">The packet to send.</param>
        public static void SendUDPData(IPEndPoint _clientEndPoint, Packet _packet)
        {
            try
            {
                if (_clientEndPoint != null)
                {
                    udpListener.BeginSend(_packet.ToArray(), _packet.Length(), _clientEndPoint, null, null);
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error sending data to {_clientEndPoint} via UDP: {_ex}");
            }
        }

        /// <summary>Initializes all necessary server data.</summary>
        private static void InitializeServerData()
        {
            for (int i = 1; i <= MaxPlayers; i++)
            {
                clients.Add(i, new Client(i, ObjectId.Empty));
            }

            fromClientpacketHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
                { (int)ClientPackets.queueRequestForRandomMatch, ServerHandle.QueueRequestForRandomMatch },
                { (int)ClientPackets.test, ServerHandle.ClientTrashRequest },
                { (int)ClientPackets.quitQueue, ServerHandle.QuitQueueRequest },
                { (int)ClientPackets.groupRequest, ServerHandle.GroupRequest },
                { (int)ClientPackets.groupDisolve, ServerHandle.GroupDisolve },
                { (int)ClientPackets.inviteFriendToGroup, ServerHandle.InviteFriendToGroup },
                { (int)ClientPackets.inviteFriendToGroupResponse, ServerHandle.InviteFriendToGroupResponse },
                { (int)ClientPackets.searchFriend, ServerHandle.SearchFriend },
                { (int)ClientPackets.searchFriendResponse, ServerHandle.SearchFriendResponse },
                { (int)ClientPackets.gameCallResponse, ServerHandle.GameFoundResponse },
            };
            Console.WriteLine("Initialized InitializeServerData packets.");
        }
        /// <summary>Initializes all necessary room data.</summary>
        public static void InitializeRoomData()
        {
            int _portCounter = 4500;
            for (int i = 1; i <= MaxRooms; i++)
            {
                _portCounter++;
                rooms.Add(i, new GameRoom(i, _portCounter));
            }
            fromClientpacketHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)GamePackets.welcomeReceived, GameHandle.WelcomeReceived },
            };
            Console.WriteLine("Initialized InitializeRoomData packets.");
        }

        public static void Stop()
        {
            tcpListener.Stop();
            udpListener.Close();
        }
    }
}
