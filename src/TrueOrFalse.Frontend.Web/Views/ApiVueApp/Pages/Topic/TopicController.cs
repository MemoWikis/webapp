
using Microsoft.AspNetCore.Mvc;
using VueApp;

public class TopicController : BaseController
{
    private readonly TopicControllerLogic _topicControllerLogic;
    private readonly CategoryViewRepo _categoryViewRepo;

    public TopicController(SessionUser sessionUser,
        TopicControllerLogic topicControllerLogic, CategoryViewRepo categoryViewRepo) : base(sessionUser)
    {
        _topicControllerLogic = topicControllerLogic;
        _categoryViewRepo = categoryViewRepo;
    }

    [HttpGet]
    public OkObjectResult GetTopic([FromRoute] int id)
    {
        var userAgent = Request.Headers["User-Agent"].ToString();
        _categoryViewRepo.AddView(userAgent, id, _sessionUser.UserId);
        var data = _topicControllerLogic.GetTopicData(id);
        return Ok(data); 
    }
}

