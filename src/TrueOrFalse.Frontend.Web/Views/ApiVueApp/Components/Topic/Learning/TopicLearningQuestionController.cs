using System.Collections.Concurrent;
using System.Linq;
using System.Web.Mvc;

[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class TopicLearningQuestionController: BaseController
{
    private readonly CategoryValuationReadingRepo _categoryValuationReadingRepo;
    private readonly CommentRepository _commentRepository;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly QuestionValuationRepo _questionValuationRepo;
    private readonly TotalsPersUserLoader _totalsPersUserLoader;

    public TopicLearningQuestionController(SessionUser sessionUser,
        CategoryValuationReadingRepo categoryValuationReadingRepo,
        CommentRepository commentRepository, 
        UserReadingRepo userReadingRepo,
        QuestionValuationRepo questionValuationRepo,
        TotalsPersUserLoader totalsPersUserLoader) : base(sessionUser)
    {
        _categoryValuationReadingRepo = categoryValuationReadingRepo;
        _commentRepository = commentRepository;
        _userReadingRepo = userReadingRepo;
        _questionValuationRepo = questionValuationRepo;
        _totalsPersUserLoader = totalsPersUserLoader;
    }
    [HttpPost]
    public JsonResult LoadQuestionData(int questionId)
    {
        var question = EntityCache.GetQuestionById(questionId);
        var author = new UserTinyModel(question.Creator);
        var authorImage = new UserImageSettings(author.Id).GetUrl_128px_square(author);
        var solution = GetQuestionSolution.Run(question);
        var answerQuestionModel = new AnswerQuestionModel(question,
            _sessionUser.UserId,
            _totalsPersUserLoader,
            _questionValuationRepo);
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
                    .Select(c => new CommentModel(c))
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
    public JsonResult GetKnowledgeStatus(int id)
    {
        var userQuestionValuation = _sessionUser.IsLoggedIn
            ? SessionUserCache.GetItem(_sessionUser.UserId, _categoryValuationReadingRepo, _userReadingRepo, _questionValuationRepo).QuestionValuations
            : new ConcurrentDictionary<int, QuestionValuationCacheItem>();

        var hasUserValuation = userQuestionValuation.ContainsKey(id) && _sessionUser.IsLoggedIn;

        return Json(hasUserValuation ? userQuestionValuation[id].KnowledgeStatus : KnowledgeStatus.NotLearned, JsonRequestBehavior.AllowGet);
    }
}