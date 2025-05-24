# FuturePrices

**FuturePrices** is a .NET Web API service that provides the price difference of cryptocurrencies over a specified period using the [Binance API](https://binance-docs.github.io/apidocs/futures/en/). The results are stored in a PostgreSQL database. Both successful and failed requests are logged.

## 📁 Solution Structure

The solution consists of 5 projects:

| Project                      |  Purpose                                                             |
|----------------------------- |----------------------------------------------------------------------|
| `FuturesPrice.WebAPI`        | Main Web API project                                                 |
| `FuturesPrice.BusinessLogic` | Business logic layer                                                 |
| `FuturesPrice.Binance`       | Binance API integration                                              |
| `FuturesPrice.DAL`           | Data access (Entity Framework Core, repositories)                    |
| `FuturesPrice.Shared`        | Shared models, DTOs, enums                                           |

## ⚙️ Installation and Running

### Prerequisites

- .NET 8 or newer
- PostgreSQL
- Visual Studio / Rider or another C# IDE
- Internet connection (for Binance API access)

### Configuration

Check the `appsettings.json` file in the `FuturesPrice.WebAPI` project:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Username=postgres;Password=password;Database=FuturesPriceDb"
  }
}
```

### Database Setup

Migration is already created. To apply the schema to the database, run:

```bash
# In Package Manager Console:
# Default project: FuturesPrice.DAL
# Startup project: FuturesPrice.WebAPI

Update-Database
```

### Running the Project

Set `FuturesPrice.WebAPI` as the startup project and press F5,  
or use:

```bash
dotnet run --project FuturesPrice.WebAPI
```

Swagger UI will be available at:  
`https://localhost:{port}/swagger`

## 📌 API Usage

### Endpoint

`GET /api/PriceDifference/calculate`

### Example Request in Swagger:

```json
{
  "symbol": "BTCUSDT",
  "startDate": "2025-01-02",
  "endDate": "2025-01-05"
}
```

### Example Response:

```json
{
  "startDate": "2025-01-02",
  "endDate": "2025-01-05",
  "priceDifference": 1300.00
}
```

### Logging

- All requests and their results are saved in the database (both successful and failed).
- In case of an error (e.g., Binance API is unavailable or invalid input), an error message is returned and stored in logs.

## 🧱 Technologies

- ASP.NET Core Web API  
- Entity Framework Core + PostgreSQL  
- Binance Futures API  
- Swagger (Swashbuckle)  
- Clean Architecture (layered approach)

## 🧑‍💻 Author

- [LevonZ] – development and architecture