using MathNet.Numerics.Statistics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestResultsApi.Data;
using TestResultsApi.Models;

namespace TestResultsApi.Controllers;

[ApiController]
[Route("results")]
public class ResultsController(TestResultsDbContext db) : ControllerBase
{

    [HttpGet("{testId}/aggregate")]
    public async Task<IActionResult> GetAggregate(string testId)
    {
        var marks = await db.TestResults
            .Where(r => r.TestId == testId)
            .Select(r => new SummaryMarks
            {
                Available = r.AvailableMarks,
                Obtained = r.ObtainedMarks,
            })
            .ToListAsync();

        var percentageScores = marks.Select(m => m.Percentage).ToList();
        
        if (percentageScores.Count == 0)
        {
            return NotFound($"No results for testId {testId}"); 
        }
        
        return Ok(new TestResultsAggregate
        {
            TestId = testId,
            Count = percentageScores.Count,
            Mean = percentageScores.Average(),
            P25 = percentageScores.Percentile(25),
            P50 = percentageScores.Percentile(50),
            P75 = percentageScores.Percentile(75),
            StdDev = percentageScores.Count > 1 ? percentageScores.StandardDeviation() : null,
        });
    }
}
