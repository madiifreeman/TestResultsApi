using Moq;
using TestResultsApi.Data;
using TestResultsApi.Models;
using TestResultsApi.Services;

namespace TestResultsApi.UnitTests;

public class TestResultsServiceTests
{
    private readonly Mock<ITestResultsRepository> _repoMock;
    private readonly TestResultsService _service;

    private const string StudentNum = "123";
    private const string TestId = "Test1";

    public TestResultsServiceTests()
    {
        _repoMock = new Mock<ITestResultsRepository>();
        _service = new TestResultsService(_repoMock.Object);
    }

    [Fact]
    public async Task ImportResults_AddsNewResults_WhenNotExists()
    {
        // Arrange

        var result = new McqTestResult
        {
            StudentNumber = StudentNum,
            TestId = TestId,
            ScannedOnRaw = "2017-12-04T12:12:10+11:00",
            SummaryMarks = new SummaryMarks { Available = 100, Obtained = 80 }
        };

        _repoMock.Setup(r => r.GetByStudentAndTestId("123", "Test1"))
                 .ReturnsAsync((TestResultEntity?)null);

        // Act
        await _service.ImportResults([result]);

        // Assert
        _repoMock.Verify(r => r.AddRange(It.Is<List<TestResultEntity>>(list =>
            list.Count == 1 &&
            list[0].StudentNumber == StudentNum &&
            list[0].ObtainedMarks == 80)), Times.Once);

        _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ImportResults_Updates_WhenHigherScoreProvided()
    {
        // Arrange
        var newScannedOn = "2017-12-04T12:12:10+11:00";
        
        var existing = new TestResultEntity
        {
            StudentNumber = StudentNum,
            TestId = TestId,
            ObtainedMarks = 60,
            AvailableMarks = 80,
            ScannedOn = DateTime.Parse(newScannedOn).ToUniversalTime().AddDays(-1)
        };

        var betterResult = new McqTestResult
        {
            StudentNumber = StudentNum,
            TestId = TestId,
            ScannedOnRaw = newScannedOn,
            SummaryMarks = new SummaryMarks { Available = 100, Obtained = 90 } // TODO: split out tests to test when just available is higher/just obtained is higher
        };

        _repoMock.Setup(r => r.GetByStudentAndTestId(StudentNum, TestId))
                 .ReturnsAsync(existing);

        // Act
        await _service.ImportResults([betterResult]);

        // Assert
        _repoMock.Verify(r => r.Update(It.Is<TestResultEntity>(e =>
            e.ObtainedMarks == 90 && 
            e.AvailableMarks == 100)), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ImportResults_DoesNotUpdate_WhenExistingIsBetter()
    {
        // Arrange
        var newScannedOn = "2017-12-04T12:12:10+11:00";
        
        var existing = new TestResultEntity
        {
            StudentNumber = StudentNum,
            TestId = TestId,
            ObtainedMarks = 95,
            AvailableMarks = 100,
            ScannedOn = DateTime.UtcNow.AddDays(-1)
        };

        var worseResult = new McqTestResult
        {
            StudentNumber = StudentNum,
            TestId = TestId,
            ScannedOnRaw = "2017-12-04T12:12:10+11:00",
            SummaryMarks = new SummaryMarks { Available = 100, Obtained = 90 }
        };

        _repoMock.Setup(r => r.GetByStudentAndTestId(StudentNum, TestId))
                 .ReturnsAsync(existing);

        await _service.ImportResults([worseResult]);

        _repoMock.Verify(r => r.Update(It.IsAny<TestResultEntity>()), Times.Never);
        _repoMock.Verify(r => r.AddRange(new List<TestResultEntity>()), Times.Once);
    }
    
    [Fact]
    public async Task ImportResults_HandlesDuplicatesInSameBatch_UsesBestResult()
    {
        // Arrange
        var worseResult = new McqTestResult
        {
            StudentNumber = StudentNum,
            TestId = TestId,
            ScannedOnRaw = "2017-12-04T12:12:10+11:00",
            SummaryMarks = new SummaryMarks { Available = 100, Obtained = 80 }
        };

        var betterResult = new McqTestResult
        {
            StudentNumber = StudentNum,
            TestId = TestId,
            ScannedOnRaw = "2017-12-04T12:12:10+11:00",
            SummaryMarks = new SummaryMarks { Available = 100, Obtained = 90 }
        };

        _repoMock.Setup(r => r.GetByStudentAndTestId("123", "Test1"))
            .ReturnsAsync((TestResultEntity?)null);

        // Act
        await _service.ImportResults([worseResult, betterResult]);

        // Assert
        _repoMock.Verify(r => r.AddRange(It.Is<List<TestResultEntity>>(list =>
            list.Count == 1 &&
            list[0].ObtainedMarks == 90 &&
            list[0].AvailableMarks == 100)), Times.Once);

        _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
