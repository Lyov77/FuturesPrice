using Serilog;
using FuturesPrice.Binance.Interfaces;
using FuturesPrice.Binance.Services;
using FuturesPrice.BusinessLogic.Interfaces;
using FuturesPrice.DAL.Interfaces;
using FuturesPrice.DAL.Services;
using FuturesPrice.DAL;
using Microsoft.EntityFrameworkCore;
using FuturesPrice.BusinessLogic.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.PostgreSQL(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        tableName: "LogEntries")
    .CreateLogger();

builder.Services.AddDbContext<AppDbContext>(options =>
       options.UseNpgsql(connectionString));

// Add services to the container.

builder.Services.AddHttpClient();
builder.Services.AddScoped<IBinanceService, BinanceService>();
builder.Services.AddScoped<IPriceQueryRepository, PriceQueryRepository>();
builder.Services.AddScoped<IPriceSaveRepository, PriceSaveRepository>();
builder.Services.AddScoped<IPriceDifferenceService, PriceDifferenceService>();
builder.Services.AddScoped<IPriceDifferenceValidator, PriceDifferenceValidator>();
builder.Services.AddScoped<IPriceDifferenceCalculator, PriceDifferenceCalculator>();
builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<ILoggingService, LoggingService>();


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

