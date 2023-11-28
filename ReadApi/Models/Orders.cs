using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ReadApi.Models
{
    public class Orders
    {
        [BsonElement("user_id")]
        public string? userId { get; set; }

        [BsonElement("date")]
        public string? date {  get; set; }

        [BsonElement("products")]
        public List<Products> products { get; set; } = null!;
    }
}
