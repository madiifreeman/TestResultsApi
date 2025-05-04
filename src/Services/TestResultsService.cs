using MathNet.Numerics.Statistics;
using TestResultsApi.Data;
using TestResultsApi.Models;

namespace TestResultsApi.Services;

public class TestResultsService(ITestResultsRepository repo)
{
    public async Task ImportResults(List<McqTestResult> results)
    {
        List<TestResultEntity> toAdd = [];
        
        foreach (var result in results)
        {
            var entity = new TestResultEntity
            {
                StudentNumber = result.StudentNumber,
                TestId = result.TestId,
                ScannedOn = result.ScannedOn,
                AvailableMarks = result.SummaryMarks.Available,
                ObtainedMarks = result.SummaryMarks.Obtained
            };
            
            var existing = await repo.GetByStudentAndTestId(entity.StudentNumber, entity.TestId);
            
            if (existing is null)
            {
                toAdd.Add(entity);
            }
            else
            {
                if (entity.ObtainedMarks > existing.ObtainedMarks || entity.AvailableMarks > existing.AvailableMarks)
                {
                    
                    existing.ObtainedMarks = Math.Max(entity.ObtainedMarks, existing.ObtainedMarks);
                    existing.AvailableMarks = Math.Max(entity.AvailableMarks, existing.AvailableMarks);
                    existing.ScannedOn = entity.ScannedOn;
                    repo.Update(existing);
                }
            }
        }
        
        await repo.AddRange(toAdd);
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