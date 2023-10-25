using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

public class VueLearningSessionResultController
    : Controller
{
    private readonly LearningSessionCache _learningSessionCache;
    private readonly ImageMetaDataReadingRepo _imageMetaDataReadingRepo;
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly QuestionReadingRepo _questionReadingRepo;

    public VueLearningSessionResultController(LearningSessionCache learningSessionCache,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        IActionContextAccessor actionContextAccessor,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        QuestionReadingRepo questionReadingRepo)
    {
        _learningSessionCache = learningSessionCache;
        _imageMetaDataReadingRepo = imageMetaDataReadingRepo;
        _actionContextAccessor = actionContextAccessor;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _questionReadingRepo = questionReadingRepo;
    }
    [HttpGet]
    public JsonResult Get()
    {
        var learningSession = _learningSessionCache.GetLearningSession();
        var model = new LearningSessionResultModel(learningSession, _httpContextAccessor, _webHostEnvironment);
        var questions = model.AnsweredStepsGrouped.Where(g => g.First().Question.Id != 0).Select(g =>
        {
            var question = g.First().Question;
            return new {
                    correctAnswerHtml = GetQuestionSolution.Run(question).GetCorrectAnswerAsHtml(),
                    id = question.Id,
                    imgUrl = GetQuestionImageFrontendData.Run(question,
                        _imageMetaDataReadingRepo, 
                        _httpContextAccessor, 
                        _webHostEnvironment,
                        _questionReadingRepo)
                        .GetImageUrl(128, true).Url,
                    title = question.GetShortTitle(),
                    steps = g.Select(s => new {
                        answerState = s.AnswerState,
                        answerAsHtml = Question.AnswersAsHtml(s.Answer, question.SolutionType)
                    }).ToArray(),
                };
        }).ToArray();

        return Json(new
        {
            uniqueQuestionCount = model.NumberUniqueQuestions,
            correct = new
            {
                percentage = model.NumberCorrectPercentage,
                count = model.NumberCorrectAnswers
            },
            correctAfterRepetition = new
            {
                percentage = model.NumberCorrectAfterRepetitionPercentage,
                count = model.NumberCorrectAfterRepetitionAnswers
            },
            wrong = new
            {
                percentage = model.NumberWrongAnswersPercentage,
                count = model.NumberWrongAnswers
            },
            notAnswered = new
            {
                percentage = model.NumberNotAnsweredPercentage,
                count = model.NumberNotAnswered
            },
            topicName = learningSession.Config.Category.Name,
            topicId = learningSession.Config.Category.Id,
            inWuwi = learningSession.Config.InWuwi,
            questions = questions
        });
    }
}