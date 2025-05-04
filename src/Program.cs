using Microsoft.EntityFrameworkCore;
using TestResultsApi.Data;
using TestResultsApi.Formatters;
using TestResultsApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, new MarkrXmlInputFormatter());
});

builder.Services.AddDbContext<TestResultsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITestResultsRepository, TestResultsRepository>();
builder.Services.AddScoped<TestResultsService>();

var app = builder.Build();

// Apply any pending DB migrations
using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<TestResultsDbContext>();
db.Database.Migrate(); 

app.MapControllers();
app.UseHttpsRedirection();

app.Run();
