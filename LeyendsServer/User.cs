using System.IO;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using MongoDB.Bson;

namespace LeyendsServer
{
    public class User
    {
        public ObjectId _id { get; set; }
        public string user_name  { get; set; }
        public string user_pass { get; set; }
        public string user_legends_nick { get; set; }
        public bool authState { get; set; }

        public User (){
            authState = false;
        }

        public override string ToString()
        {
            return "User: " + _id.ToString() + " " + user_name + " " + user_pass + " " + user_legends_nick;
        }

    }
}

