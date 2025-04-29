using System.Collections.Generic;
using System.Threading.Tasks;
using PortfolioTrackerApi.Models;


namespace PortfolioTrackerApi.Services
{
    public interface IAlphaVantageService
    {
        Task<List<StockMatch>> SearchSymbolAsync(string keyword);  // Should return Task<List<StockMatch>>
        Task<StockQuote> GetLatestPriceAsync(string symbol); 

    }
}
