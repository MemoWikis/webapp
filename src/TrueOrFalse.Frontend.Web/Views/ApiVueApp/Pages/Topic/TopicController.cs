using Microsoft.AspNetCore.Mvc;

public class TopicController : BaseController
{
    private readonly TopicDataManager _topicDataManager;
    private readonly CategoryViewRepo _categoryViewRepo;

    public TopicController(
        SessionUser sessionUser,
        TopicDataManager topicDataManager,
        CategoryViewRepo categoryViewRepo) : base(sessionUser)
    {
        _topicDataManager = topicDataManager;
        _categoryViewRepo = categoryViewRepo;
    }

    [HttpGet]
    public TopicDataManager.TopicDataResult GetTopic([FromRoute] int id)
    {
        var userAgent = Request.Headers["User-Agent"].ToString();
        _categoryViewRepo.AddView(userAgent, id, _sessionUser.UserId);
        var data = _topicDataManager.GetTopicData(id);
        return data;
    }
}