namespace TestResultsApi.Models;

public class TestResultEntity
{
    public int Id { get; set; }
    public string StudentNumber { get; set; }
    public string TestId { get; set; }
    public DateTime ScannedOn { get; set; }
    public int AvailableMarks { get; set; }
    public int ObtainedMarks { get; set; }
}