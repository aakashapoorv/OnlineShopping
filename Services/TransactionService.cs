using System.Threading.Tasks;
using MongoDB.Driver;
using OnlineShopping.Models;

public class TransactionService
{
    private readonly IMongoCollection<Order> _ordersCollection;
    private readonly IMongoCollection<Cart> _cartsCollection;
    private readonly IMongoCollection<Product> _productsCollection;
    private readonly IMongoCollection<User> _usersCollection;
    private readonly IMongoCollection<Transaction> _transactionsCollection;

    public TransactionService(IMongoClient client)
    {
        var database = client.GetDatabase("OnlineShopping");
        _ordersCollection = database.GetCollection<Order>("Orders");
        _cartsCollection = database.GetCollection<Cart>("Carts");
        _productsCollection = database.GetCollection<Product>("Products");
        _usersCollection = database.GetCollection<User>("Users");
        _transactionsCollection = database.GetCollection<Transaction>("Transactions");
    }

    public async Task<Transaction> SaveTransactionByOrderIdAsync(string orderId)
    {
        var order = await _ordersCollection.Find(o => o.Id == orderId).FirstOrDefaultAsync();
        if (order == null)
        {
            return null; // Order not found
        }

        var cart = await _cartsCollection.Find(c => c.Id == order.CartId).FirstOrDefaultAsync();
        if (cart == null)
        {
            return null; // Cart not found
        }

        var productIds = cart.Products.Select(item => item.ProductId).ToList();
        var products = await _productsCollection.Find(p => productIds.Contains(p.Id)).ToListAsync();
        if (products == null || products.Count == 0)
        {
            return null; // Products not found
        }

        var user = await _usersCollection.Find(u => u.Id == order.UserId).FirstOrDefaultAsync();
        if (user == null)
        {
            return null; // User not found
        }

        var transaction = new Transaction
        {
            UserId = user.Id,
            UserEmail = user.Email,
            OrderId = order.Id,
            User = user,
            Order = order,
            Cart = cart,
            Products = products,
            CreatedOn = DateTime.Now
        };

        await _transactionsCollection.InsertOneAsync(transaction); // Save the transaction to the Transactions collection

        return transaction;
    }
}
