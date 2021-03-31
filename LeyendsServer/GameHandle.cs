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
            string _msg = _packet.ReadString();
            Console.WriteLine(_msg);
        }
    }
}
