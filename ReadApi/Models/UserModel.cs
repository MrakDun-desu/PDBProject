using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReadApi.Models
{
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("user_name")]
        public string? userName {  get; set; }

        [BsonElement("email")]
        public string? email {  get; set; }

        [BsonElement("address")]
        public string? address { get; set; }

        [BsonElement("orders")]
        public List<Orders>? orders { get; set; }
    }
}
