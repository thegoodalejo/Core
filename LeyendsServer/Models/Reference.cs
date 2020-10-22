using MongoDB.Bson;

namespace LeyendsServer
{
    public class Reference
    {
        public int user_position_server  { get; set; }

        public Reference (int _user_position_server){
            user_position_server = _user_position_server;
        }

        public override string ToString()
        {
            return "Reference: " + user_position_server;
        }

    }
}

