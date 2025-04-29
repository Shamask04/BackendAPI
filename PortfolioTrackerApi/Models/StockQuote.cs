public class StockQuote
{
    public string Symbol { get; set; } = string.Empty; // <-- Default value to remove warning
    public decimal CurrentPrice { get; set; }
    public decimal OpenPrice { get; set; }
    public decimal GainLoss { get; set; }
    public decimal GainLossPercentage { get; set; }
}
