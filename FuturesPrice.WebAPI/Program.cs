/*var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();*/

using FuturesPrice.Binance.Interfaces;
using FuturesPrice.Binance.Services;
using FuturesPrice.BusinessLogic.Interfaces;
using FuturesPrice.BusinessLogic;
using FuturesPrice.DAL.Interfaces;
using FuturesPrice.DAL.Services;
using FuturesPrice.DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Connection string configuration. 
string dbConnectionString = builder.Configuration.GetConnectionString("SqlServerConnection"); // switch to "PostgreSqlConnection"



// Add services to the container.

builder.Services.AddHttpClient();
builder.Services.AddScoped<IBinanceService, BinanceService>();
builder.Services.AddScoped<IPriceRepository, PriceRepository>();
builder.Services.AddScoped<IPriceDifferenceService, PriceDifferenceService>();


if (dbConnectionString.Contains("Server"))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(dbConnectionString)); // SQL Server
}
else
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(dbConnectionString)); // PostgreSQL
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

