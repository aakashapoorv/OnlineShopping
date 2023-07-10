using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OnlineShopping.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShopping.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMongoClient _mongoClient;

        public ProductsController(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            var database = _mongoClient.GetDatabase("OnlineShopping");
            var collection = database.GetCollection<Product>("Products");

            return await collection.Find(FilterDefinition<Product>.Empty).ToListAsync();
        }
    }
}
