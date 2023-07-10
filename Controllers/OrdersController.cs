using OnlineShopping.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace OnlineShopping.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMongoCollection<Order> _orders;
        private readonly TransactionService _transactionService;

        public OrdersController(IMongoClient client, TransactionService transactionService)
        {
            var database = client.GetDatabase("OnlineShopping");
            _orders = database.GetCollection<Order>("Orders");
            _transactionService = transactionService;
        }

        [HttpGet]
        public IEnumerable<Order> Get()
        {
            return _orders.Find(order => true).ToList();
        }

        [HttpPost]
        public Order Create(Order order)
        {
            order.Id = ObjectId.GenerateNewId().ToString();
            _orders.InsertOneAsync(order);
            var transaction = _transactionService.SaveTransactionByOrderIdAsync(order.Id);
            return order;
        }
    }
}
