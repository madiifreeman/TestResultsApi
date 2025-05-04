using MathNet.Numerics.Statistics;
using TestResultsApi.Data;
using TestResultsApi.Models;

namespace TestResultsApi.Services;

public class TestResultsService(ITestResultsRepository repo)
{
    public async Task ImportResults(List<McqTestResult> results)
    {
        var entities = results.Select(r => new TestResultEntity
        {
            StudentNumber = r.StudentNumber,
            TestId = r.TestId,
            ScannedOn = r.ScannedOn,
            AvailableMarks = r.SummaryMarks.Available,
            ObtainedMarks = r.SummaryMarks.Obtained
        });
        
        await repo.AddRange(entities);
        await repo.SaveChangesAsync();
    }
    
    public async Task<TestResultsAggregate?> GetAggregateResults(string testId)
    {
        var scores = await repo.GetScoresByTestId(testId);

        if (scores.Count == 0)
        {
            return null;
        }

        return new TestResultsAggregate
        {
            TestId = testId,
            Count = scores.Count,
            Mean = scores.Average(),
            P25 = scores.Percentile(25),
            P50 = scores.Percentile(50),
            P75 = scores.Percentile(75),
            StdDev = scores.Count > 1 ? scores.StandardDeviation() : null,
        };
    }
}