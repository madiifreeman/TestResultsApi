using System.Net;
using System.Text;

namespace TestResultsApi.ComponentTests;

public class ResultsTests: IAsyncLifetime
{
    private readonly HttpClient _client = new()
    {
        BaseAddress = new Uri("http://localhost:4567") // TODO: pull from appsettings
    };

    private const string TestIdWithExistingResults = "1111";
    
    public async Task InitializeAsync()
    {
        // Make sure a record for this test id exists in the DB
        var xml = $"""
                       <mcq-test-results>
                           <mcq-test-result scanned-on="2017-12-04T12:12:10+11:00">
                               <first-name>Jane</first-name>
                               <last-name>Austen</last-name>
                               <student-number>521585128</student-number>
                               <test-id>{TestIdWithExistingResults}</test-id>
                               <summary-marks available="20" obtained="13" />
                           </mcq-test-result>
                       </mcq-test-results>
                   """;
        
        var content = new StringContent(xml, Encoding.UTF8, "text/xml+markr");
        var response = await _client.PostAsync("/import", content);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to seed test result data into DB");
        }
    }

    public Task DisposeAsync() => Task.CompletedTask;
    
    [Fact]
    public async Task GetResultsForTestId_Returns200()
    {
        var response = await _client.GetAsync($"/results/{TestIdWithExistingResults}/aggregate");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task GetResultsForNonExistentTestId_Returns404()
    {
        var response = await _client.GetAsync("/results/0000/aggregate");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}