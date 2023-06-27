using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Web;

[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class VueLearningSessionResultController: Controller
{
    private readonly LearningSessionCache _learningSessionCache;

    public VueLearningSessionResultController(LearningSessionCache learningSessionCache)
    {
        _learningSessionCache = learningSessionCache;
    }

    [HttpGet]
    public JsonResult Get()
    {
        
        var learningSession = _learningSessionCache.GetLearningSession();
        var model = new LearningSessionResultModel(learningSession);
        var questions = model.AnsweredStepsGrouped.Where(g => g.First().Question.Id != 0).Select(g =>
        {
            var question = g.First().Question;
            return new {
                    correctAnswerHtml = GetQuestionSolution.Run(question).GetCorrectAnswerAsHtml(),
                    id = question.Id,
                    imgUrl = GetQuestionImageFrontendData.Run(question).GetImageUrl(128, true,
                        false, ImageType.Question).Url,
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
            encodedTopicName = UriSanitizer.Run(learningSession.Config.Category.Name),
            topicId = learningSession.Config.Category.Id,
            inWuwi = learningSession.Config.InWuwi,
            questions = questions,
        }, JsonRequestBehavior.AllowGet);
    }
}