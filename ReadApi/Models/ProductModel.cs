using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ReadApi.Models
{
    public class ProductModel
    {
        [BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("product_name")]
        public string productName { get; set; } = null!;

        [BsonElement("category_name")]
        public string categoryName { get; set; } = null!;

        [BsonElement("price")]
        [BsonRepresentation(BsonType.Int32)]
        public int price { get; set; }

        [BsonElement("description")]
        public string description { get; set; } = null!;

        [BsonElement("stock_count")]
        [BsonRepresentation(BsonType.Int32)]
        public int stockCount {  get; set; }

    }
}
