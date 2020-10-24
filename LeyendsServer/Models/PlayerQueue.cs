namespace LeyendsServer
{
    public class PlayerQueue
    {
        public int id { get; set; }
        public bool isGroupLeader { get; set; }
        public bool queueAcepted { get; set; }
        public PlayerQueue(int _id)
        {
            id = _id;
            queueAcepted = false;
        }
    }
}

