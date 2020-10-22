using MongoDB.Bson;

namespace LeyendsServer
{
    public class User
    {
        public ObjectId _id { get; set; }
        public string user_name  { get; set; }
        public string user_pass { get; set; }
        public string user_legends_nick { get; set; }
        public bool acc_aviable { get; set; }
        public int acc_Status { get; set; }

        public User (){
            acc_aviable = false;
            user_legends_nick = "Null";
        }
        public User(bool state){
            acc_aviable = true;
            user_legends_nick = "Null";
        }

        public override string ToString()
        {
            return "User: " + _id.ToString() + " " + user_name + " " + user_pass + " " + user_legends_nick;
        }

    }
}

