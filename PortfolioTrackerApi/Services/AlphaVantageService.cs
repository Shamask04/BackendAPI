using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using PortfolioTrackerApi.Models;
using System.Linq;

namespace PortfolioTrackerApi.Services
{
    public class AlphaVantageService : IAlphaVantageService
{
    private readonly HttpClient _httpClient;
    private const string ApiKey = "UCWZPNO6K2SZUR92"; // for search
    private const string BaseUrl = "https://www.alphavantage.co/query?function=SYMBOL_SEARCH";
 
    public AlphaVantageService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
 
    // For searching ticker symbol
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
                    MatchScore = decimal.Parse(item.GetProperty("9. matchScore").GetString())
                });
            }
        }
 
        return matches;
    }
 
    // For fetching real-time stock quote
    public async Task<StockQuote> GetLatestPriceAsync(string symbol)
{
    const string IntradayApiKey = "FOP521B655SCXWON"; // your correct API key

    var url = $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={symbol}&interval=5min&apikey={IntradayApiKey}";
    var response = await _httpClient.GetAsync(url);

    if (!response.IsSuccessStatusCode)
        return null;

    var jsonString = await response.Content.ReadAsStringAsync();
    var jsonDoc = JsonDocument.Parse(jsonString);

    if (!jsonDoc.RootElement.TryGetProperty("Time Series (5min)", out var timeSeries))
        return null;

    var latestEntry = timeSeries.EnumerateObject().FirstOrDefault();
    if (latestEntry.Equals(default(JsonProperty)))
        return null;

    var latestData = latestEntry.Value;

    decimal currentPrice = decimal.Parse(latestData.GetProperty("4. close").GetString() ?? "0");
    decimal openPrice = decimal.Parse(latestData.GetProperty("1. open").GetString() ?? "0");


    decimal gainLoss = currentPrice - openPrice;
    decimal gainLossPercent = (gainLoss / openPrice) * 100;

    return new StockQuote
    {
        Symbol = symbol,
        CurrentPrice = currentPrice,
        OpenPrice = openPrice,
        GainLoss = gainLoss,
        GainLossPercentage = gainLossPercent
    };
}
}
}
