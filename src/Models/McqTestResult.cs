namespace TestResultsApi.Models;

using System;
using System.Xml.Serialization;

public class McqTestResult
{
    [XmlAttribute("scanned-on")]
    public string ScannedOnRaw { get; set; }

    [XmlIgnore]
    public DateTime ScannedOn => DateTime.Parse(ScannedOnRaw);

    [XmlElement("first-name")]
    public string FirstName { get; set; }

    [XmlElement("last-name")]
    public string LastName { get; set; }

    [XmlElement("student-number")]
    public string StudentNumber { get; set; }

    [XmlElement("test-id")]
    public string TestId { get; set; }

    [XmlElement("summary-marks")]
    public SummaryMarks SummaryMarks { get; set; }
}

public class SummaryMarks
{
    [XmlAttribute("available")]
    public int Available { get; set; }

    [XmlAttribute("obtained")]
    public int Obtained { get; set; }
}
