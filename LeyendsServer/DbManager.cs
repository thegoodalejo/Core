using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LeyendsServer
{
    class DbManager
    {

        private const string DB_CONNECTION = "mongodb://127.0.0.1:27017";
        private const string LEYENDS_DB = "LeyendsDb";
        private const string COLLECTION_USERS = "Users";
        private const string COLLECTION_USERS_USER_NAME = "user_name";
        private const string COLLECTION_USERS_USER_PASS = "user_pass";

        public static bool Auth(string _userName, string _pass)
        {
            var filter = Builders<BsonDocument>.Filter.Eq(COLLECTION_USERS_USER_NAME, _userName);
            try
            {
                MongoClient dbClient = new MongoClient(DB_CONNECTION);
                var database = dbClient.GetDatabase(LEYENDS_DB);
                var collection = database.GetCollection<BsonDocument>(COLLECTION_USERS);
                var pass = collection.Find(filter).FirstOrDefault().GetValue(COLLECTION_USERS_USER_PASS);
                return pass==_pass;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);
                return false;
            }
        }


        private void simpleExample()
        {
            try
            {
                MongoClient dbClient = new MongoClient(DB_CONNECTION);

                var dbList = dbClient.ListDatabases().ToList();

                Console.WriteLine("The list of databases on this server is: ");
                foreach (var db in dbList)
                {
                    Console.WriteLine(db);
                }
                Console.WriteLine("\n");
                var database = dbClient.GetDatabase("LeyendsDb");
                Console.WriteLine("The list of collections on this database is: ");
                foreach (var item in database.ListCollectionsAsync().Result.ToListAsync<BsonDocument>().Result)
                {
                    Console.WriteLine(item.ToString() + "\n");
                }

                var collection = database.GetCollection<BsonDocument>("Users");
                var document = new BsonDocument
            {
                { "user_name", "Admin" },
                { "user_pass", "abc"},
                { "user_preferences", new BsonArray
                    {
                    new BsonDocument{
                        {"type", "exam"},
                        {"score", 88.12334193287023 } },
                    new BsonDocument{
                        {"type", "quiz"},
                        {"score", 74.92381029342834 } },
                    new BsonDocument{
                        {"type", "homework"},
                        {"score", 89.97929384290324 } },
                    new BsonDocument{
                        {"type", "homework"},
                        {"score", 82.12931030513218 } }
                    }
                }
            };

                collection.InsertOne(document);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);
            }
        }
    }
}

