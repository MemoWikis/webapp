using Markdig.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
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
    public JsonResult GetTopic([FromRoute] int id)
    {
        var userAgent = Request.Headers["User-Agent"].ToString();
        _categoryViewRepo.AddView(userAgent, id, _sessionUser.UserId);

        return Json(_topicControllerLogic.GetTopicData(id));
    }
}

