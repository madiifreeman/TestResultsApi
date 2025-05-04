namespace TestResultsApi.Models;

using System.Xml.Serialization;

[XmlRoot("mcq-test-results")]
public class McqTestResults
{
    [XmlElement("mcq-test-result")]
    public List<McqTestResult> Results { get; set; }
}
