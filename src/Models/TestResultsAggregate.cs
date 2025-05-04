namespace TestResultsApi.Models;

public class TestResultsAggregate
{
    public string TestId { get; set; }
    public int Count { get; set; }
    public double Mean { get; set; }
    public double P25 { get; set; }
    public double P50 { get; set; }
    public double P75 { get; set; }
    
    public double? StdDev { get; set; }
}