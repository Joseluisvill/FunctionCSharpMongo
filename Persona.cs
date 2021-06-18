using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace My.Functions
{
    public class Persona
    {
        [BsonId]
        public ObjectId id{get;set;}
        [BsonElement("nombre")]
        public string nombre{get;set;}

        [BsonElement("edad")]
        public int edad{get;set;}
    }
}