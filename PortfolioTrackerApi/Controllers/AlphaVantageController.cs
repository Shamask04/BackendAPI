using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using PortfolioTrackerApi.Services;

[ApiController]
[Route("api/[controller]")]
public class AlphaVantageController : ControllerBase
{
    private readonly IAlphaVantageService _alphaVantageService;

    public AlphaVantageController(IAlphaVantageService alphaVantageService)
    {
        _alphaVantageService = alphaVantageService;
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<StockMatch>>> Search([FromQuery] string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return BadRequest("Keyword is required.");

        var results = await _alphaVantageService.SearchSymbolAsync(keyword);
        return Ok(results);
    }
}
