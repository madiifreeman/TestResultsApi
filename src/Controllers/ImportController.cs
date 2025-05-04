using TestResultsApi.Models;

namespace TestResultsApi.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ImportController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Import([FromBody] McqTestResults results)
    {
        return Ok(results.Results[0].FirstName);
    }
}
