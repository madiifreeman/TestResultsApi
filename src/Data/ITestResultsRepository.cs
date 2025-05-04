using TestResultsApi.Models;

namespace TestResultsApi.Data;

public interface ITestResultsRepository
{
    Task AddRange(IEnumerable<TestResultEntity> result);
    Task<List<double>> GetScoresByTestId(string testId);
    Task<TestResultEntity?> GetByStudentAndTestId(string studentId, string testId);
    Task SaveChangesAsync();
    void Update(TestResultEntity result);
}