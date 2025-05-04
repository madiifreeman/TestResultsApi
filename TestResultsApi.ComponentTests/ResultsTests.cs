using System.Net;

namespace TestResultsApi.ComponentTests;

public class ResultsTests
{
    private readonly HttpClient _client = new()
    {
        BaseAddress = new Uri("http://localhost:4567") // TODO: pull from appsettings
    };
    
    [Fact]
    public async Task GetResultsForNonExistentTestId_Returns404()
    {
        var response = await _client.GetAsync("/results/0000/aggregate");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}