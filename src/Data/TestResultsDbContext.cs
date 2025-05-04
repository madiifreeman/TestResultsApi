using Microsoft.EntityFrameworkCore;
using TestResultsApi.Models;

namespace TestResultsApi.Data;

public class TestResultsDbContext : DbContext
{
    public TestResultsDbContext(DbContextOptions<TestResultsDbContext> options) : base(options) {}

    public DbSet<TestResultEntity> TestResults { get; set; }
}