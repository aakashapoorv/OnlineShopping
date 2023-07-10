using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OnlineShopping.Models
{
    public class Transaction
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("userEmail")]
        public string UserEmail { get; set; }

        [BsonElement("orderId")]
        public string OrderId { get; set; }

        [BsonElement("user")]
        public User User { get; set; }

        [BsonElement("order")]
        public Order Order { get; set; }

        [BsonElement("cart")]
        public Cart Cart { get; set; }

        [BsonElement("products")]
        public List<Product> Products { get; set; }

        [BsonElement("CreatedOn")]
        public DateTime CreatedOn { get; set; }
    }
}
