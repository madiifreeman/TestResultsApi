using System.ComponentModel.DataAnnotations;

namespace TestResultsApi.Models;

using System;
using System.Xml.Serialization;

public class McqTestResult
{
    [Required]
    [XmlAttribute("scanned-on")]
    public string ScannedOnRaw { get; set; }

    [XmlIgnore] // TODO: separate concerns of DTO and model so we don't have these ignored member vars
    public DateTime ScannedOn => DateTime.Parse(ScannedOnRaw).ToUniversalTime();

    [XmlElement("first-name")]
    public string FirstName { get; set; }

    [XmlElement("last-name")]
    public string LastName { get; set; }
    
    [Required]
    [XmlElement("student-number")]
    public string StudentNumber { get; set; }
    
    [Required]
    [XmlElement("test-id")]
    public string TestId { get; set; }

    [Required]
    [XmlElement("summary-marks")] 
    public SummaryMarks SummaryMarks { get; set; }
}

public class SummaryMarks
{
    [Required]
    [XmlAttribute("available")]
    public int Available { get; set; }

    [Required]
    [XmlAttribute("obtained")]
    public int Obtained { get; set; }
    
    [XmlIgnore] // TODO: separate concerns of DTO and model so we don't have these ignored member vars
    public double Percentage => Available > 0
        ? (double)Obtained / Available * 100
        : 0;
}
