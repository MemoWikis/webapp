using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class TopicController : ControllerBase
{
    [HttpGet("{id}")]
    public TopicModel GetTopic(int id)
    {
        return new TopicModel
        {
            Id = id,
            Name = $"Name {id}"
        };
    }
}

public class TopicModel
{
    public int Id { get; set; }
    public string Name { get; set; }
}