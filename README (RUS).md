# FuturePrices

**FuturePrices** — это Web API-сервис на .NET, который позволяет получать разницу цен по криптовалютам за указанный период с использованием [Binance API](https://binance-docs.github.io/apidocs/futures/en/). Результаты сохраняются в базе данных PostgreSQL. Также ведется логирование успешных и неуспешных запросов.

## 📁 Структура решения

Решение состоит из 5 проектов:

| Проект                        | Назначение                                                            |
|-------------------------      |-----------------------------------------------------------------------|
| `FuturesPrice.WebAPI`         | Основной веб API проект                                               |
| `FuturesPrice.BusinessLogic   | Бизнес-логика обработки данных                                        |
| `FuturesPrice.Binance`        | Работа с Binance API                                                  |
| `FuturesPrice.DAL`            | Доступ к данным (Entity Framework Core, репозитории)                  |
| `FuturesPrice.Shared`         | Общие модели, DTO, перечисления                                       |

## ⚙️ Установка и запуск

### Предварительные требования

- .NET 8 или новее
- PostgreSQL
- Visual Studio / Rider или другой C# IDE
- Подключение к интернету (для доступа к Binance API)

### Конфигурация

Проверьте файл `appsettings.json` в проекте `FuturesPrice.WebAPI`:

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

### Настройка базы данных

Миграция уже создана. Для применения схемы к базе данных выполните:

```bash
# В Package Manager Console:
# Default project: FuturesPrice.DAL
# Startup project: FuturesPrice.WebAPI

Update-Database
```

### Запуск проекта

Выберите `FuturesPrice.WebAPI` как стартовый проект и нажмите F5  
или используйте:

```bash
dotnet run --project FuturesPrice.WebAPI
```

Swagger будет доступен по адресу:  
`https://localhost:{порт}/swagger`

## 📌 Использование API

### Endpoint

`GET /api/PriceDifference/calculate`

### Пример запроса в Swagger:

```json
{
  "symbol": "BTCUSDT",
  "startDate": "2025-01-02",
  "endDate": "2025-01-05"
}
```

### Пример ответа:

```json
{
   "startDate": "2025-01-02",
   "endDate": "2025-01-05"
   "priceDifference": 1300.00,
  }
```

### Логирование

- Все запросы и их результаты сохраняются в БД (успешные и неуспешные).
- В случае ошибки (например, недоступность Binance API или некорректные данные) возвращается описание ошибки и она сохраняется в журнале.

## 🧱 Технологии

- ASP.NET Core Web API
- Entity Framework Core + PostgreSQL
- Binance Futures API
- Swagger (Swashbuckle)
- Clean Architecture (слоистая архитектура)

## 🧑‍💻 Автор

- [LevonZ] – разработка и архитектура