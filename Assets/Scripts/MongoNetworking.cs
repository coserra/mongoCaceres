using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MongoNetworking : MonoBehaviour
{
    MongoClient client = new MongoClient("mongodb+srv://admin:admin@test.7yomk.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;
    // Start is called before the first frame update
    void Start()
    {
        database = client.GetDatabase("sample_geospatial");
        collection = database.GetCollection<BsonDocument>("shipwrecks");

        //var document = new BsonDocument { { "juan", "22" } };
        //collection.InsertOne(document);
        //var algo = collection.Find<BsonDocument>("value:Bulevar");
        //Debug.Log("Recuperado"+algo.ToJson());

        //var dbList = client.ListDatabases().ToList();
        //Debug.Log("The list of databases on this server is: ");
        //foreach (var db in dbList)
        //{
        //    Debug.Log(db);
        //}

        var firstDocument = collection.Find(new BsonDocument()).FirstOrDefault();
        Debug.Log(firstDocument.ToString());

        var filter = Builders<BsonDocument>.Filter.Eq("chart","US,U1,graph,DNC H1409860");

        var studentDocument = collection.Find(filter).ToList();
        foreach (var element in studentDocument)
        {
            Debug.Log(element.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
