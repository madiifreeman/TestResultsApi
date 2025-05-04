namespace TestResultsApi.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ImportController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Import()
    {
        return Ok("hello!");
    }
}
