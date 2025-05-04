using TestResultsApi.Data;
using TestResultsApi.Models;

namespace TestResultsApi.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ImportController(TestResultsDbContext db) : ControllerBase
{
    
    [HttpPost]
    public async Task<IActionResult> Import([FromBody] McqTestResults results)
    {
        var entities = results.Results.Select(r => new TestResultEntity
        {
            StudentNumber = r.StudentNumber,
            TestId = r.TestId,
            ScannedOn = r.ScannedOn,
            AvailableMarks = r.SummaryMarks.Available,
            ObtainedMarks = r.SummaryMarks.Obtained
        });

        await db.TestResults.AddRangeAsync(entities);
        await db.SaveChangesAsync();

        return Ok();
    }
}
