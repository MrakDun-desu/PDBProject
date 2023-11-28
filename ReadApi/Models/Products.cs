using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ReadApi.Models
{
    public class Products
    {
        [BsonElement("_id")]
        public string? productId { get; set; }

        [BsonElement("product_count")]
        public int productCount { get; set; }
    }
}
