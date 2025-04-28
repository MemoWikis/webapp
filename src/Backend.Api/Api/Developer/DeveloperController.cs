using Microsoft.Extensions.Hosting;

[ApiController]
[Route("[controller]/[action]/{id?}")]
public class DeveloperController : ControllerBase
{
    [HttpPost]
    [AccessOnlyLocal]
    public IActionResult CreateSampleData()
    {
        if (!App.Environment.IsDevelopment())
        {
            return BadRequest("This method can only be called in a Development environment.");
        }
        
        
        
        return Ok("Sample data created successfully.");
    }
}