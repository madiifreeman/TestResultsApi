using System.Text;

namespace TestResultsApi.ComponentTests;

public class ImportTests
{
    private readonly HttpClient _client;

    public ImportTests()
    {
        _client = new HttpClient
        {
            BaseAddress = new System.Uri("http://localhost:5252") // TODO: pull from appsettings
        };
    }
    
    [Fact]
    public async Task PostImport_WithValidXml_Returns200Ok()
    {
        var xml = """
                      <mcq-test-results>
                          <mcq-test-result scanned-on="2017-12-04T12:12:10+11:00">
                              <first-name>Jane</first-name>
                              <last-name>Austen</last-name>
                              <student-number>521585128</student-number>
                              <test-id>1234</test-id>
                              <summary-marks available="20" obtained="13" />
                          </mcq-test-result>
                      </mcq-test-results>
                  """;

        var content = new StringContent(xml, Encoding.UTF8, "text/xml+markr");
        var response = await _client.PostAsync("/import", content);

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }
}
