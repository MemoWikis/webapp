using System.Linq;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;
public class VueQuestionController : BaseController
{
    private readonly QuestionRepo _questionRepo;

    public VueQuestionController(QuestionRepo questionRepo)
    {
        _questionRepo = questionRepo;
    }

    public JsonResult LoadQuestion(int questionId)
    {
        var userQuestionValuation = SessionUserCache.GetItem(SessionUser.UserId).QuestionValuations;
        var q = EntityCache.GetQuestionById(questionId);
        var question = new QuestionListJson.Question();
        question.Id = q.Id;
        question.Title = q.Text;
        question.LinkToQuestion = Links.GetUrl(q);
        question.ImageData = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(q.Id, ImageType.Question)).GetImageUrl(40, true).Url;
        question.LinkToQuestion = Links.GetUrl(q);
        question.LinkToEditQuestion = Links.EditQuestion(q.Text, q.Id);
        question.LinkToQuestionVersions = Links.QuestionHistory(q.Id);
        question.LinkToComment = Links.GetUrl(q) + "#JumpLabel";
        question.CorrectnessProbability = q.CorrectnessProbability;
        question.Visibility = q.Visibility;

        var learningSession = LearningSessionCache.GetLearningSession();
        if (learningSession != null)
        {
            var steps = learningSession.Steps;
            var index = steps.IndexOf(s => s.Question.Id == q.Id);
            question.SessionIndex = index;
        }

        if (userQuestionValuation.ContainsKey(q.Id) && SessionUser.User != null)
        {
            question.CorrectnessProbability = userQuestionValuation[q.Id].CorrectnessProbability;
            question.IsInWishknowledge = userQuestionValuation[q.Id].IsInWishKnowledge;
            question.HasPersonalAnswer = userQuestionValuation[q.Id].CorrectnessProbabilityAnswerCount > 0;
        }

        return Json(question);
    }

    [HttpPost]
    public JsonResult GetData(int id)
    {
        var question = EntityCache.GetQuestionById(id);
        var categoryController = new CategoryController();
        var solution = question.SolutionType == SolutionType.FlashCard ? GetQuestionSolution.Run(question).GetCorrectAnswerAsHtml() : question.Solution;
        var topicsVisibleToCurrentUser =
            question.Categories.Where(PermissionCheck.CanView).Distinct();

        var json = new JsonResult
        {
            Data = new
            {
                SolutionType = (int)question.SolutionType,
                Solution = solution,
                SolutionMetadataJson = question.SolutionMetadataJson,
                Text = question.TextHtml,
                TextExtended = question.TextExtendedHtml,
                TopicIds = topicsVisibleToCurrentUser.Select(c => c.Id).ToList(),
                DescriptionHtml = question.DescriptionHtml,
                Topics = topicsVisibleToCurrentUser.Select(c => categoryController.FillMiniCategoryItem(c)),
                LicenseId = question.LicenseId,
                Visibility = question.Visibility,
            }
        };

        return json;
    }

    [HttpPost]
    public JsonResult DeleteDetails(int questionId)
    {
        var question = _questionRepo.GetById(questionId);
        var canBeDeleted = QuestionDelete.CanBeDeleted(question.Creator.Id, question);

        return new JsonResult
        {
            Data = new
            {
                questionTitle = question.Text.TruncateAtWord(90),
                totalAnswers = question.TotalAnswers(),
                canNotBeDeleted = !canBeDeleted.Yes,
                wuwiCount = canBeDeleted.WuwiCount,
                hasRights = canBeDeleted.HasRights
            }
        };
    }

    [HttpPost]
    public JsonResult Delete(int questionId, int sessionIndex)
    {
        QuestionDelete.Run(questionId);
        LearningSessionCache.RemoveQuestionFromLearningSession(sessionIndex, questionId);
        return new JsonResult
        {
            Data = new
            {
                sessionIndex,
                questionId
            }
        };
    }

    [RedirectToErrorPage_IfNotLoggedIn]
    public ActionResult Restore(int questionId, int questionChangeId)
    {
        RestoreQuestion.Run(questionChangeId, this.User_());

        var question = Sl.QuestionRepo.GetById(questionId);
        return Redirect(Links.AnswerQuestion(question));
    }
}