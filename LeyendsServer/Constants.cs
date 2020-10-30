using System;
using System.Collections.Generic;
using System.Text;

namespace LeyendsServer
{
    public enum Status
    {
        Offline = 0,
        Online,
        Away
    }

    class Constants
    {
        private const int TICKS_PER_SEC_LOGIN = 5;
        private const int TICKS_PER_SEC_COMMANDS = 1;
        private const int TICKS_PER_SEC_QUEUE = 1;
        public const float MS_PER_TICK_LOGIN = 1000f / TICKS_PER_SEC_LOGIN;
        public const float MS_PER_TICK_TICKS_PER_SEC_COMMANDS = 1000f / TICKS_PER_SEC_COMMANDS;
        public const float MS_PER_TICK_TICKS_PER_SEC_QUEUE = 1000f / TICKS_PER_SEC_QUEUE;


    }

    class QueueType
    {
        public const string NON = "NoQueue";
        public const string RANDOM = "Random";
    }
}
