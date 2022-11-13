using Microsoft.AspNetCore.Mvc;

namespace SAS22.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BacktestSessionController : ControllerBase
{
    internal AppDb Db { get; set; }

    private readonly ILogger<BacktestSessionController> _logger;

    public BacktestSessionController(ILogger<BacktestSessionController> logger, AppDb db)
    {
        _logger = logger;
        Db = db;
    }


    [HttpGet("{sessionId}")]
    public async Task<IActionResult> GetOne(int sessionId)
    {
        await Db.Connection.OpenAsync();
        var query = new BacktestSessionQuery(Db);
        var result = await query.FindOneAsync(sessionId);
        if (result is null)
            return new NotFoundResult();
        return new OkObjectResult(result);
    }
}
