using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class TopicController(
    SessionUser _sessionUser,
    CategoryViewRepo _categoryViewRepo,
    PermissionCheck _permissionCheck,
    KnowledgeSummaryLoader _knowledgeSummaryLoader,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    IHttpContextAccessor _httpContextAccessor,
    QuestionReadingRepo _questionReadingRepo)
    : Controller
{
    [HttpGet]
    public TopicDataManager.TopicDataResult GetTopic([FromRoute] int id)
    {
        var userAgent = Request.Headers["User-Agent"].ToString();
        _categoryViewRepo.AddView(userAgent, id, _sessionUser.UserId);
        var data = new TopicDataManager(
            _sessionUser, 
            _permissionCheck, 
            _knowledgeSummaryLoader, 
            _categoryViewRepo,
            _imageMetaDataReadingRepo, 
            _httpContextAccessor, 
            _questionReadingRepo)
            .GetTopicData(id);
        return data;
    }
}