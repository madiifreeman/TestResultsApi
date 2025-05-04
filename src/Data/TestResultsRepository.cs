using Microsoft.EntityFrameworkCore;
using TestResultsApi.Models;

namespace TestResultsApi.Data;

public class TestResultsRepository(TestResultsDbContext db): ITestResultsRepository
{
    public async Task AddRange(IEnumerable<TestResultEntity> results)
    {
        await db.TestResults.AddRangeAsync(results);
    }

    public async Task<List<double>> GetScoresByTestId(string testId)
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
        return percentageScores;
    }

    public async Task SaveChangesAsync()
    {
        await db.SaveChangesAsync();
    }
}