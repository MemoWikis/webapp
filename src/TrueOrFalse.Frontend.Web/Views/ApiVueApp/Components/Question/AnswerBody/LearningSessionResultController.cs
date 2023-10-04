using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

public class VueLearningSessionResultController(LearningSessionCache learningSessionCache,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        IActionContextAccessor actionContextAccessor,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment, 
        QuestionReadingRepo questionReadingRepo)
    : Controller
{
    [HttpGet]
    public JsonResult Get()
    {
        
        var learningSession = learningSessionCache.GetLearningSession();
        var model = new LearningSessionResultModel(learningSession, httpContextAccessor, webHostEnvironment);
        var questions = model.AnsweredStepsGrouped.Where(g => g.First().Question.Id != 0).Select(g =>
        {
            var question = g.First().Question;
            return new {
                    correctAnswerHtml = GetQuestionSolution.Run(question).GetCorrectAnswerAsHtml(),
                    id = question.Id,
                    imgUrl = GetQuestionImageFrontendData.Run(question,
                        imageMetaDataReadingRepo, 
                        httpContextAccessor, 
                        webHostEnvironment,
                        questionReadingRepo)
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