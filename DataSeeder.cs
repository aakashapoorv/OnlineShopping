using System;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using OnlineShopping.Models;

public class DataSeeder
{
    private readonly IMongoClient _client;

    public DataSeeder(IMongoClient client)
    {
        _client = client;
    }

    public void SeedData()
    {
        var database = _client.GetDatabase("OnlineShopping");

        if (!IsCollectionEmpty(database, "Products"))
        {
            var products = new Product[]
            {
                new Product{Id = ObjectId.GenerateNewId().ToString(), Name="Product 1", Price=10, Description="This is product 1"},
                new Product{Id = ObjectId.GenerateNewId().ToString(), Name="Product 2", Price=20, Description="This is product 2"},
                new Product{Id = ObjectId.GenerateNewId().ToString(), Name="Product 3", Price=30, Description="This is product 3"},
            };

            database.GetCollection<Product>("Products").InsertMany(products);
        }

        if (!IsCollectionEmpty(database, "Users"))
        {
            var users = new User[]
            {
                new User { Id = ObjectId.GenerateNewId().ToString(), Username = "user1", Email = "user1@example.com", Password = "hashed_password1", FirstName = "John", LastName = "Doe" },
                new User { Id = ObjectId.GenerateNewId().ToString(), Username = "user2", Email = "user2@example.com", Password = "hashed_password2", FirstName = "Jane", LastName = "Doe" }
            };

            database.GetCollection<User>("Users").InsertMany(users);
        }

        var _users = database.GetCollection<User>("Users").Find(_ => true).ToList();
        var _products = database.GetCollection<Product>("Products").Find(_ => true).ToList();


         if (!IsCollectionEmpty(database, "Carts"))
        {
            var cartItems = new List<CartItem>
            {
                new CartItem { ProductId = _products[0].Id, Quantity = 2 },
                new CartItem { ProductId = _products[1].Id, Quantity = 1 }
            };

            var cart = new Cart
            {
                Products = cartItems,
                UserId = _users[0].Id
            };

            database.GetCollection<Cart>("Carts").InsertOne(cart);
        }

        var _carts = database.GetCollection<Cart>("Carts").Find(_ => true).ToList();
        

        if (!IsCollectionEmpty(database, "Orders"))
        {
            var order = new Order {CartId = _carts[0].Id, PaymentMethod = "Credit Card", UserId = _users[0].Id};
            database.GetCollection<Order>("Orders").InsertOne(order);
        }
    }

    private bool IsCollectionEmpty(IMongoDatabase database, string collectionName)
    {
        var filter = Builders<BsonDocument>.Filter.Empty;
        var result = database.GetCollection<BsonDocument>(collectionName).Find(filter).FirstOrDefault();
        return result != null;
    }
}
