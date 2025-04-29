using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PortfolioTrackerApi.Services
{
    public class AlphaVantageService : IAlphaVantageService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "UCWZPNO6K2SZUR92";
        private const string BaseUrl = "https://www.alphavantage.co/query?function=SYMBOL_SEARCH";

        public AlphaVantageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<StockMatch>> SearchSymbolAsync(string keyword)
        {
            var url = $"{BaseUrl}&keywords={keyword}&apikey={ApiKey}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var matches = new List<StockMatch>();

            if (doc.RootElement.TryGetProperty("bestMatches", out var bestMatches))
            {
                foreach (var item in bestMatches.EnumerateArray())
                {
                    matches.Add(new StockMatch
                    {
                        Symbol     = item.GetProperty("1. symbol").GetString(),
                        Name       = item.GetProperty("2. name").GetString(),
                        Region     = item.GetProperty("4. region").GetString(),
                        MatchScore = item.GetProperty("9. matchScore").GetString()
                    });
                }
            }

            return matches; // Returning a list of StockMatch
        }
    }

    public class StockMatch
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string MatchScore { get; set; }
    }
}
