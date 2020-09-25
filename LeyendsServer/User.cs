using System.IO;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LeyendsServer
{
    public class User
    {
        public string token { get; set; }
        public string user { get; set; }
        public string pass { get; set; }

        public override string ToString()
        {
            return "User: " + token + " " + user + " " + pass;
        }

        public void things()
        {
            List<User> userList = new List<User>();
            FileStream fs = new FileStream(Environment.CurrentDirectory + "\\DataBase\\user.xml", FileMode.Append, FileAccess.Write);
            XmlSerializer dbCreator = new XmlSerializer(typeof(List<User>));
            userList.Add(new User() { token = "ABC", user = "Admon2", pass = "pass" });
            using (fs)
            {
                dbCreator.Serialize(fs, userList);
            }
            XmlSerializer dbReader = new XmlSerializer(typeof(List<User>));
            FileStream fs1 = new FileStream(Environment.CurrentDirectory + "\\DataBase\\user.xml", FileMode.Open, FileAccess.Read);
            using (fs1)
            {
                userList = dbReader.Deserialize(fs1) as List<User>;
            }

            foreach (var item in userList) Console.WriteLine(item);
        }

        public void dbConn()
        {

            /*var client = new MongoClient("mongodb+srv://5pGIeE1HJBLGZrt3:<password>@cluster0.nhawu.gcp.mongodb.net/<dbname>?retryWrites=true&w=majority");
            var database = client.GetDatabase("test");*/

        }

    }
}

