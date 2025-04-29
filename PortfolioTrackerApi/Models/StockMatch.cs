namespace PortfolioTrackerApi.Models
{
    public class StockMatch
    {
        public string? Symbol { get;set;}
        public string? Name { get;set;}
        public string? Type { get;set;}
        public string? Region { get;set;}
        public decimal? MatchScore { get;set;}
    }
}
