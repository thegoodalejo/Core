using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace LeyendsServer
{
    class GameHandle
    {
        public static void WelcomeReceived(Packet _packet)//ID:1
        {
            Console.WriteLine($"WelcomeReceived");
            string _msg = _packet.ReadString();
            Console.WriteLine(_msg);
            Guid guid = Guid.NewGuid();
            string str = guid.ToString();
            Console.WriteLine($"Temp Token {str}");

        }
    }
}
