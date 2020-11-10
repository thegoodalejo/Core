using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace LeyendsServer
{
    class DbManager
    {

        private const string DB_CONNECTION = "mongodb://127.0.0.1:27017";
        private const string LEYENDS_DB = "LeyendsDb";
        private const string COLLECTION_USERS = "Users";
        private const string COLLECTION_USERS_USER_NAME = "user_name";
        private const string COLLECTION_USERS_USER_NICK_NAME = "user_legends_nick";
        private const string COLLECTION_USERS_USER_OID = "_id";
        public static User Auth(int _toClient, string _userName, string _pass)
        {
            User userResponse;
            var filterUsers = Builders<BsonDocument>.Filter.Eq(COLLECTION_USERS_USER_NAME, _userName);
            try
            {
                MongoClient dbClient = new MongoClient(DB_CONNECTION);
                var database = dbClient.GetDatabase(LEYENDS_DB);
                var collection = database.GetCollection<BsonDocument>(COLLECTION_USERS);
                var result = collection.Find(filterUsers).FirstOrDefault();
                userResponse = BsonSerializer.Deserialize<User>(result);
                //Aca ya tengo la respuesta de base de datos
                if (userResponse.user_pass == _pass && userResponse.acc_Status != (int)Status.Online)
                {
                    List<FriendReference> userFriends = new List<FriendReference>();
                    foreach (ObjectId item in userResponse.user_friends)
                    {
                        userFriends.Add(new FriendReference(item));
                    }
                    Server.clients[_toClient].token = userResponse._id;
                    Server.clients[_toClient].nickName = userResponse.user_legends_nick;
                    Server.clients[_toClient].user_friends = userFriends;
                    UpdateState(userResponse._id, (int)Status.Online, _toClient);
                    return userResponse;
                }
                else
                {
                    return new User();
                }
            } //xiEUli^WkkA38cTnKnWQ
            catch (Exception ex)
            {
                Console.WriteLine("Error Auth DB:" + ex.Message);
                return new User();
            }
        }

        public static void UpdateState(ObjectId _token, int _status, int _slot)
        {
            MongoClient dbClient = new MongoClient(DB_CONNECTION);
            var database = dbClient.GetDatabase(LEYENDS_DB);
            var collection = database.GetCollection<User>(COLLECTION_USERS);
            var update = Builders<User>.Update.Set(a => a.acc_Status, _status).Set(a => a.server_slot, _slot);
            var result = collection.UpdateOne(model => model._id == _token, update);
        }
        public static void UpdateStateAll()
        {
            MongoClient dbClient = new MongoClient(DB_CONNECTION);
            var database = dbClient.GetDatabase(LEYENDS_DB);
            var collection = database.GetCollection<User>(COLLECTION_USERS);
            var update = Builders<User>.Update.Set(a => a.acc_Status, (int)Status.Offline).Set(a => a.server_slot, (int)Status.Offline);
            var result = collection.UpdateMany(FilterDefinition<User>.Empty, update);
        }
        public static List<User> FriendList(int _toClient)
        {
            var filterFriends = Builders<User>.Filter.In(x => x._id, Server.clients[_toClient].GetFriendsKeys());
            MongoClient dbClient = new MongoClient(DB_CONNECTION);
            var database = dbClient.GetDatabase(LEYENDS_DB);
            var collection = database.GetCollection<User>(COLLECTION_USERS);
            return collection.Find(filterFriends).ToListAsync().Result;
        }
        //Builders<User>.Filter.And es para varias condiciones ojo ahi se ponen separadas por comas, tipo lista de condiciones
        public static async Task AddFriend(int _fromClient, int _toClient)
        {
            MongoClient dbClient = new MongoClient(DB_CONNECTION);
            var database = dbClient.GetDatabase(LEYENDS_DB);
            var collection = database.GetCollection<User>(COLLECTION_USERS);
            ObjectId _fromToken = Server.clients[_fromClient].token;
            ObjectId _toToken = Server.clients[_toClient].token;
            var filter1 = Builders<User>.Filter.Where(x => x._id == _fromToken);
            var update1 = Builders<User>.Update.Push(x => x.user_friends, _toToken);
            await collection.FindOneAndUpdateAsync(filter1, update1);
            var filter2 = Builders<User>.Filter.Where(x => x._id == _toToken);
            var update2 = Builders<User>.Update.Push(x => x.user_friends, _fromToken);
            await collection.FindOneAndUpdateAsync(filter2, update2);
            
        }
    }
}

