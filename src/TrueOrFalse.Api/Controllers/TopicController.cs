using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class TopicController : ControllerBase
{
    [HttpGet("{id}")]
    public HeadModel GetHead(int id)
    {
        var category = GetCategory(id);

        return new HeadModel
        {
            Id = id,
            Name = $"Name {id}"
        };
    }

    private static CategoryCacheItem GetCategory(int id)
    {
        var category = EntityCache.GetCategory(id);
        if (!PermissionCheck.CanView(category))
            category = null;
        return category;
    }

    [HttpGet("{id}")]
    public FirstTabModel GetMainTab(int id)
    {
        return new FirstTabModel
        {
        };
    }

    [HttpGet("{id}")]
    public QuestionTabModel GetQuestionTab(int id)
    {
        return new QuestionTabModel
        {
        };
    }
}

public class HeadModel
{
    public int Id;

    public string? MetaTitle;
    public string? MetaDescription;

    public string? Name;
    public string? Description;
}

public class FirstTabModel
{
    public string Text;
}

public class QuestionTabModel
{

}