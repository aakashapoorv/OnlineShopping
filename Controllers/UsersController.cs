using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OnlineShopping.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShopping.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMongoClient _mongoClient;

        public UsersController(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetUsers()
        {
            var database = _mongoClient.GetDatabase("OnlineShopping");
            var collection = database.GetCollection<User>("Users");

            return await collection.Find(FilterDefinition<User>.Empty).ToListAsync();
        }
    }
}
