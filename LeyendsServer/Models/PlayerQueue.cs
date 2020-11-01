namespace LeyendsServer
{
    public class PlayerQueue
    {
        public int id { get; set; }
        public string nick_name { get; set; }
        public bool isGroupLeader { get; set; }
        public bool queueAcepted { get; set; }
        public PlayerQueue(int _id, string _nick_name)
        {
            id = _id;
            nick_name = _nick_name;
            queueAcepted = false;
        }
    }
}

