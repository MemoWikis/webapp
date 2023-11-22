using System.Collections.Concurrent;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class TopicLearningQuestionController: BaseController
{
    private readonly CommentRepository _commentRepository;
    private readonly TotalsPersUserLoader _totalsPersUserLoader;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly SessionUserCache _sessionUserCache;

    public TopicLearningQuestionController(SessionUser sessionUser,
        CommentRepository commentRepository, 
        TotalsPersUserLoader totalsPersUserLoader,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        SessionUserCache sessionUserCache) : base(sessionUser)
    {
        _commentRepository = commentRepository;
        _totalsPersUserLoader = totalsPersUserLoader;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _sessionUserCache = sessionUserCache;
    }

    [HttpPost]
    public JsonResult LoadQuestionData([FromRoute] int id)
    {
        var question = EntityCache.GetQuestionById(id);
        var author = new UserTinyModel(question.Creator);
        var authorImage = new UserImageSettings(author.Id, _httpContextAccessor)
            .GetUrl_128px_square(author);
        var solution = GetQuestionSolution.Run(question);
        var answerQuestionModel = new AnswerQuestionModel(question,
            _sessionUser.UserId,
            _totalsPersUserLoader,
            _sessionUserCache);
        var history = answerQuestionModel.HistoryAndProbability.AnswerHistory;

        var json = Json(new RequestResult
        {
            success = true,
            data = new
            {
                answer = solution.GetCorrectAnswerAsHtml(),
                extendedAnswer = question.DescriptionHtml ?? "",
                authorName = author.Name,
                authorId = author.Id,
                authorImageUrl = authorImage.Url,
                extendedQuestion = question.TextExtendedHtml ?? "",
                commentCount = _commentRepository.GetForDisplay(question.Id)
                    .Where(c => !c.IsSettled)
                    .Select(c => new CommentModel(c, _httpContextAccessor, _webHostEnvironment))
                    .ToList()
                    .Count(),
                isCreator = author.Id == _sessionUser.UserId,
                answerCount = history.TimesAnsweredUser,
                correctAnswerCount = history.TimesAnsweredUserTrue,
                wrongAnswerCount = history.TimesAnsweredUserWrong,
                canBeEdited = question.Creator?.Id == _sessionUser.UserId || IsInstallationAdmin,
                title = question.Text,
                visibility = question.Visibility
            }
        });

        return json;
    }

    [HttpGet]
    public JsonResult GetKnowledgeStatus([FromRoute] int id)
    {
        var userQuestionValuation = _sessionUser.IsLoggedIn
            ? _sessionUserCache.GetItem(_sessionUser.UserId).QuestionValuations
            : new ConcurrentDictionary<int, QuestionValuationCacheItem>();

        var hasUserValuation = userQuestionValuation.ContainsKey(id) && _sessionUser.IsLoggedIn;

        return Json(hasUserValuation ? userQuestionValuation[id].KnowledgeStatus : KnowledgeStatus.NotLearned);
    }
}