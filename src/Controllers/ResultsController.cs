using Microsoft.AspNetCore.Mvc;
using TestResultsApi.Services;

namespace TestResultsApi.Controllers;

[ApiController]
[Route("results")]
public class ResultsController(TestResultsService testResultsService) : ControllerBase
{

    [HttpGet("{testId}/aggregate")]
    public async Task<IActionResult> GetAggregate(string testId)
    {
        var aggregateResult = await testResultsService.GetAggregateResults(testId);

        if (aggregateResult is null)
        {
            return NotFound("No results exist for this test id");
        }
        
        return Ok(aggregateResult);
    }
}
