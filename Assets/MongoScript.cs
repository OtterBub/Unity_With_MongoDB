using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Wrappers;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;
using System;
using Random = UnityEngine.Random;

public class MongoScript : MonoBehaviour
{
	public Text list;
	public Text InputText;

	public class Order
	{
		public enum STATE : byte
		{
			NEW = 0,
			CANCELLED = 1,
			CMOPLETE = 2
		}

		public ObjectId Id { get; set; }
		public string account { get; set; }
		public string sku { get; set; }
		public STATE state { get; set; }
		public DateTime created { get; set; }
		public string token { get; set; }
	}

	public class Post
	{
		public ObjectId Id { get; set; }
		public string title { get; set; }
		public string url { get; set; }
		public ObjectId userId { get; set; }
		public bool flagged { get; set; }
		public string author { get; set; }
	}

	Vector3 DestPos = new Vector3( 3, 3, 3 );
	MongoCollection<Order> oders;
	MongoCollection<BsonDocument> posts;
	MongoClient client;
	MongoServer server;
	MongoDatabase db;
	int counter = 0;
	// Use this for initialization
	void Start( )
	{

		//string connString = "mongodb://localhost:3001";
		string connString = "mongodb://175.126.82.238:27017";
		client = new MongoClient( connString );
		server = client.GetServer( );
		server.Connect( new TimeSpan(3000) );
		db = server.GetDatabase( "meteor" );
		oders = db.GetCollection<Order>( "oders" );

		posts = db.GetCollection<BsonDocument>( "posts" );
		MongoCursor<BsonDocument> cursor = posts.FindAll( );
		//Post findonePost = posts.FindOne();

		//Debug.Log( findonePost.title );
		//list.text = "";
		//foreach( BsonDocument post in cursor ) {
		//	foreach( BsonElement val in post ) {
		//		if( !val.Value.IsBsonNull )
		//		{
		//			Debug.Log( val.Name + "::" + val.Value.ToString() );
		//		}
		//	}
		//	list.text += post.GetElement("title").Value.AsString;
		//}
		
		Refresh();

		var document = new BsonDocument
		{
			{ "name", "MongoDB" },
			{ "type", "Database" },
			{ "count", 1 },
			{ "info", new BsonDocument 
					{
						{ "x", 203 },
						{ "y", 102 }
					}
			}
		};
		
		var collection = db.GetCollection<BsonDocument>("Test");

		//collection.Insert<BsonDocument>( document );

		//for( int i = 0; i < 10; i++ )
		//{
		//	InsertValue();
		//}
	}

	private void InsertValue( )
	{
		var order = new Order
		{
			account = "sdfsdfsdf",
			sku = "skuskusku33",
			state = ( Order.STATE )Random.Range( 0, 3 ),
			created = DateTime.Now,
			token = "sdfsdfdseee"
		};


		UnityThreadHelper.TaskDistributor.Dispatch( ( ) =>
		{
			oders.Insert( order );
			System.Threading.Thread.Sleep( 10 );
			if( System.Threading.Interlocked.Increment( ref counter ) < 100000 )
			{
				UnityThreadHelper.Dispatcher.Dispatch( InsertValue );
			}
		} );
	}
	// Update is called once per frame
	void Update( )
	{
		//gameObject.transform.position += ( DestPos - gameObject.transform.position ) * ( Time.deltaTime );
	}

	public void Insert()
	{
		if( InputText.text.Length <= 0 )
		{
			return;
		}

		BsonDocument doc = new BsonDocument 
		{
			{ "title", InputText.text }
		};

		posts.Insert( doc );
	}

	public void Refresh()
	{
		MongoCursor<BsonDocument> cursor = posts.FindAll( );
		list.text = "";
		foreach( BsonDocument post in cursor )
		{
			BsonValue val;
			
			if( post.TryGetValue( "title", out val ) )
				list.text += val.AsString + "\n";
		}
	}
}
