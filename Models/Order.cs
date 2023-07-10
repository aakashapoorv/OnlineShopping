using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OnlineShopping.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        
        [BsonElement("cartId")]
        public string CartId { get; set; }
        
        [BsonElement("paymentMethod")]
        public string PaymentMethod { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
    }
}
