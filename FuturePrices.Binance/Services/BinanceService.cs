using FuturesPrice.Binance.Interfaces;
using Newtonsoft.Json;
using System.Net.Http;

namespace FuturesPrice.Binance.Services
{
    public class BinanceService : IBinanceService
    {
        private readonly HttpClient _httpClient;

        public BinanceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetFuturePriceAsync(string symbol, DateTime timestamp)
        {
            try
            {
                // Преобразуем timestamp в формат Unix timestamp для запроса
                var unixTimestamp = new DateTimeOffset(timestamp).ToUnixTimeMilliseconds();
                var url = $"https://api.binance.com/api/v3/klines?symbol={symbol}&interval=1h&startTime={unixTimestamp}&limit=1";

                var response = await _httpClient.GetStringAsync(url);
                var data = JsonConvert.DeserializeObject<List<List<object>>>(response);

                if (data == null || !data.Any())
                    throw new Exception("No data received from Binance API");

                var price = Convert.ToDecimal(data[0][4]); // Цена закрытия последней свечи
                return price;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching price for {symbol} at {timestamp}: {ex.Message}");
            }
        }
    }
}
