using MongoDB.Bson;

namespace LeyendsServer
{
    public class FriendReference
    {
        public ObjectId _oid { get; set; }
        public int server_slot { get; set; }
        public string user_legends_nick { get; set; }

        public FriendReference(ObjectId oid, int _server_slot, string _user_legends_nick)
        {
            _oid = oid;
            user_legends_nick = _user_legends_nick;
            server_slot = _server_slot;
        }
    }
}

