using System;
using System.Collections.Generic;
using System.Text;

namespace LeyendsServer
{
    class ServerLogic
    {
        /// <summary>Runs all game logic.</summary>
        public static void Update()
        {
            ThreadManager.UpdateMain();
        }
    }
}
