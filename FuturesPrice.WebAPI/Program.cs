using Serilog;
using Serilog.Events;
using FuturesPrice.Binance.Interfaces;
using FuturesPrice.Binance.Services;
using FuturesPrice.BusinessLogic.Interfaces;
using FuturesPrice.DAL.Interfaces;
using FuturesPrice.DAL.Services;
using FuturesPrice.DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using FuturesPrice.BusinessLogic.Services;
using Serilog.Sinks.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.PostgreSQL(
        connectionString: builder.Configuration.GetConnectionString("PostgreSqlConnection"),
        tableName: "LogEntries")
    .CreateLogger();

// Connection string configuration. 
string dbConnectionString = builder.Configuration.GetConnectionString("PostgreSqlConnection"); // "PostgreSqlConnection" è "SqlServerConnection"



// Add services to the container.

builder.Services.AddHttpClient();
builder.Services.AddScoped<IBinanceService, BinanceService>();
builder.Services.AddScoped<IPriceRepository, PriceRepository>();
builder.Services.AddScoped<IPriceDifferenceService, PriceDifferenceService>();
builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<ILoggingService, LoggingService>(); 




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

builder.Host.UseSerilog();  

var app = builder.Build();

app.UseSerilogRequestLogging();

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

