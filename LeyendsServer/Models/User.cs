using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LeyendsServer
{
    public class User
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public string user_name { get; set; }
        public string user_pass { get; set; }
        public int server_slot { get; set; }
        public string user_legends_nick { get; set; }
        public bool acc_aviable { get; set; }
        public int acc_Status { get; set; }
        public List<ObjectId> user_friends { get; set; }

        public User()
        {
            acc_aviable = false;
            user_legends_nick = "Null";
        }
        public User(bool state)
        {
            acc_aviable = true;
            user_legends_nick = "Null";
        }

        public FriendReference GetFriendReference()
        {
            return new FriendReference(_id, server_slot, user_legends_nick);
        }
        public override string ToString()
        {
            return $"User: {_id.ToString()} {server_slot} {user_legends_nick}.";
        }

    }
}

