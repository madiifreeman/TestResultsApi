using TestResultsApi.Data;
using TestResultsApi.Models;
using TestResultsApi.Services;

namespace TestResultsApi.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ImportController(TestResultsService testResultsService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Import([FromBody] McqTestResults results)
    {
        await testResultsService.ImportResults(results.Results);

        return Ok();
    }
}
