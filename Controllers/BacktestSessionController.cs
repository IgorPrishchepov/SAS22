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

    // GET api/backtestsession/{session id}
    [HttpGet("{sessionId}")]
    public async Task<IActionResult> GetOne(int sessionId)
    {
        await Db.Connection.OpenAsync();
        var query = new BacktestSessionApi(Db);
        var result = await query.FindOneAsync(sessionId);
        if (result is null)
            return new NotFoundResult();
        return new OkObjectResult(result);
    }

    // POST api/backtestsession
    [HttpPost]
    public async Task<IActionResult> PostOne([FromBody] BacktestSession body)
    {
        await Db.Connection.OpenAsync();
        body.Db = Db;
        await body.InsertAsync();
        return new OkObjectResult(body);
    }

    // DELETE api/backtestsession/{session id}
    [HttpDelete]
    public async Task<IActionResult> DeleteOne(int sessionId)
    {
        await Db.Connection.OpenAsync();      
        var api = new BacktestSessionApi(Db);
        var result = await api.FindOneAsync(sessionId);
        await result.DeleteAsync();
        return new OkResult();
    }
}
