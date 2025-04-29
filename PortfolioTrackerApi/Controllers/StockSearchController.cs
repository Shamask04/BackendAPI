using Microsoft.AspNetCore.Mvc;
using PortfolioTrackerApi.Services;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class StockSearchController : ControllerBase
{
    private readonly AlphaVantageService _alphaVantageService;

    public StockSearchController(AlphaVantageService alphaVantageService)
    {
        _alphaVantageService = alphaVantageService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return BadRequest("Keyword is required");

        var results = await _alphaVantageService.SearchSymbolAsync(keyword);
        return Ok(results);
    }
}
