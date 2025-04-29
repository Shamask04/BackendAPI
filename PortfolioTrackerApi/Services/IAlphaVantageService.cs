using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortfolioTrackerApi.Services
{
    public interface IAlphaVantageService
    {
        Task<List<StockMatch>> SearchSymbolAsync(string keyword);  // Should return Task<List<StockMatch>>
    }
}
