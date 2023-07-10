using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;
using OnlineShopping.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using SendGrid;
using SendGrid.Helpers.Mail;

public class MonthlyEmailService
{
    private readonly IMongoCollection<Transaction> _transactionsCollection;
    private readonly IMongoCollection<User> _usersCollection;

    public MonthlyEmailService(IMongoClient client)
    {
        var database = client.GetDatabase("OnlineShopping"); 
        _transactionsCollection = database.GetCollection<Transaction>("Transactions");
        _usersCollection = database.GetCollection<User>("Users");
    }

    public async Task SendMonthlyEmail()
    {
        var users = _usersCollection.Find(user => true).ToList();

        foreach (var user in users)
        {
            var filter = Builders<Transaction>.Filter.Where(t => t.UserId == user.Id && t.CreatedOn >= DateTime.Now.AddMonths(-1));
            var transactions = _transactionsCollection.Find(filter).ToList();

            if (transactions.Count > 0)
            {
                var fileName = $"MonthlyStatement-{user.Id}.pdf";
                var document = new Document();
                PdfWriter.GetInstance(document, new FileStream(fileName, FileMode.Create));
                document.Open();

                foreach (var transaction in transactions)
                {
                    document.Add(new Paragraph($"Transaction ID: {transaction.Id}"));
                    document.Add(new Paragraph($"Order ID: {transaction.OrderId}"));
                    document.Add(new Paragraph("Cart items:"));

                    foreach (var cartItem in transaction.Cart.Products)
                    {
                        document.Add(new Paragraph($"Product ID: {cartItem.ProductId}, Quantity: {cartItem.Quantity}"));
                    }

                    document.Add(new Paragraph("------------------------------"));
                }

                document.Close();

                var sendgridApiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
                var apiKey = sendgridApiKey;
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("test@example.com", "Example User");
                var to = new EmailAddress(user.Email);
                var subject = "Your Monthly Statement";
                var plainTextContent = "Please find attached your monthly statement.";
                var htmlContent = "<p>Please find attached your monthly statement.</p>";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var bytes = File.ReadAllBytes(fileName);
                var fileContent = Convert.ToBase64String(bytes);
                msg.AddAttachment(fileName, fileContent);
                var response = await client.SendEmailAsync(msg);

                if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
                {
                    Console.WriteLine("Failed to send email: " + response.StatusCode);
                }
            }
        }
    }


}
