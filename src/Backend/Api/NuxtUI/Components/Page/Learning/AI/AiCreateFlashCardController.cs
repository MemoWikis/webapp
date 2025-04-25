using Newtonsoft.Json;
using System.Text.RegularExpressions;
using TrueOrFalse;

public class AiCreateFlashCardController(
    SessionUser _sessionUser,
    QuestionInKnowledge _questionInKnowledge,
    PageRepository pageRepository,
    UserReadingRepo _userReadingRepo,
    QuestionWritingRepo _questionWritingRepo,
    Logg _logg,
    LearningSessionCreator _learningSessionCreator,
    LearningSessionCache _learningSessionCache) : ApiBaseController
{
    public readonly record struct FlashCardJson(string front, string back);

    public readonly record struct CreateRequest(
        int PageId,
        FlashCardJson[] flashcards,
        int LastIndex,
        LearningSessionConfig SessionConfig);

    public readonly record struct CreateResponse(bool Success, int[]? Ids = null, string? MessageKey = null, int? LastIndex = null);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public CreateResponse Create([FromBody] CreateRequest request)
    {
        if (!_sessionUser.IsLoggedIn)
            return new CreateResponse(false);

        var ids = new List<int>();
        int? lastQuestionId = null;
        var lastIndex = request.LastIndex;
        foreach (var flashcardJson in request.flashcards)
        {
            var id = CreateFlashCard(flashcardJson, request.PageId);
            if (id != null && id > 0)
            {
                ids.Add((int)id);

                var questionCacheItem = EntityCache.GetQuestion((int)id);

                _learningSessionCreator.InsertNewQuestionToLearningSession(
                    questionCacheItem,
                    lastIndex,
                    request.SessionConfig);
                lastIndex++;
                lastQuestionId = id;
            }
        }

        if (lastQuestionId != null)
        {
            var learningSession = _learningSessionCache.GetLearningSession();
            if (learningSession != null)
                lastIndex = learningSession.Steps.IndexOf(s => s.Question.Id == lastQuestionId);
        }

        return new CreateResponse
        {
            Success = true,
            Ids = ids.ToArray(),
            LastIndex = lastIndex
        };
    }

    private int? CreateFlashCard(FlashCardJson json, int pageId)
    {
        var safeText = GetSafeText(json.front);

        if (string.IsNullOrEmpty(safeText) || string.IsNullOrEmpty(json.back))
            return null;

        var limitCheck = new LimitCheck(_logg, _sessionUser);

        if (!limitCheck.CanSavePrivateQuestion() && EntityCache.GetPage(pageId).Visibility != PageVisibility.Public)
            return null;

        var question = new Question
        {
            TextHtml = json.front,
            Text = safeText,
            SolutionType = SolutionType.FlashCard
        };

        var solutionModelFlashCard = new QuestionSolutionFlashCard
        {
            Text = json.back
        };

        question.Solution = JsonConvert.SerializeObject(solutionModelFlashCard);
        question.Creator = _userReadingRepo.GetById(_sessionUser.UserId)!;
        question.Pages = new List<Page>
        {
            pageRepository.GetById(pageId)
        };
        question.Visibility = limitCheck.CanSavePrivateQuestion() ? QuestionVisibility.Private : QuestionVisibility.Public;
        question.License = LicenseQuestionRepo.GetDefaultLicense();

        _questionWritingRepo.Create(question, pageRepository);
        _questionInKnowledge.Pin(Convert.ToInt32(question.Id), _sessionUser.UserId);

        return question.Id;
    }

    private string GetSafeText(string text)
    {
        return Regex.Replace(text, "<.*?>", "");
    }
}