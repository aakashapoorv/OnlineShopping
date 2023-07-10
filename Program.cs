using System;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure MongoDB service
var mongoDbConnectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING");
builder.Services.AddSingleton<IMongoClient>(ServiceProvider =>
{
    var settings = MongoClientSettings.FromConnectionString(mongoDbConnectionString);
    return new MongoClient(settings);
});
builder.Services.AddSingleton<MonthlyEmailService>();


builder.Services.AddScoped<TransactionService>(); 

var app = builder.Build();

var seeder = new DataSeeder(app.Services.GetRequiredService<IMongoClient>());
seeder.SeedData();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var emailService = app.Services.GetRequiredService<MonthlyEmailService>();

var timer = new Timer((e) =>
{
    if (DateTime.Now.Day == 30)
    {
        emailService.SendMonthlyEmail();
    }
}, null, TimeSpan.Zero, TimeSpan.FromDays(1));


app.Run();
