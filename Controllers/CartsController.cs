using OnlineShopping.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace OnlineShopping.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CartsController : ControllerBase
    {
        private readonly IMongoCollection<Cart> _carts;

        public CartsController(IMongoClient client)
        {
            var database = client.GetDatabase("OnlineShopping");
            _carts = database.GetCollection<Cart>("Carts");
        }

        [HttpGet]
        public IEnumerable<Cart> Get()
        {
            return _carts.Find(cart => true).ToList();
        }

        [HttpPost]
        public Cart Create(Cart cart)
        {
            cart.Id = ObjectId.GenerateNewId().ToString();
            _carts.InsertOne(cart);
            return cart;
        }

        [HttpPost("addproduct/{cartId}")]
        public async Task<IActionResult> AddProductToCart(string cartId, [FromBody] string productId)
        {
            var filter = Builders<Cart>.Filter.Eq(c => c.Id, cartId);
            var update = Builders<Cart>.Update.AddToSet(c => c.Products, new CartItem { ProductId = productId, Quantity = 1 });

            var result = await _carts.UpdateOneAsync(filter, update);
            if (result.ModifiedCount > 0)
                return Ok();

            return NotFound();
        }
    }
}
