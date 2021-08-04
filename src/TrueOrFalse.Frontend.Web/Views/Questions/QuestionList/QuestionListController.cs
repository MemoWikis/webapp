using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;

[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class QuestionListController : BaseController
{
 
    [HttpPost]
    public JsonResult LoadQuestions(int itemCountPerPage, int pageNumber)
    {
        var newQuestionList = QuestionListModel.PopulateQuestionsOnPage(pageNumber, itemCountPerPage, IsLoggedIn);
        return Json(newQuestionList);
    }

    [HttpPost]
    public JsonResult RenderSessionHeaderWithQuestionId(int questionId, int categoryId)
    {
        var question = EntityCache.GetQuestionById(questionId);
        var categoryCacheItem = EntityCache.GetCategoryCacheItem(categoryId);
        var categoryModel = new CategoryModel(categoryCacheItem);
        var html = ViewRenderer.RenderPartialView("~/Views/Questions/Answer/LearningSession/LearningSessionHeader.ascx", new AnswerQuestionModel(question, null, false, categoryModel), ControllerContext);
        return Json(new {
            html
        });
    }

    [HttpPost]
    public JsonResult LoadQuestionBody(int questionId)
    {
        var question = EntityCache.GetQuestionById(questionId);
        var author = new UserTinyModel(question.Creator);
        var authorImage = new UserImageSettings(author.Id).GetUrl_128px_square(author);
        var solution = GetQuestionSolution.Run(question);
        var answerQuestionModel = new AnswerQuestionModel(question, true);
        var history = answerQuestionModel.HistoryAndProbability.AnswerHistory;

        var json = Json(new
        {
            answer = solution.GetCorrectAnswerAsHtml(),
            extendedAnswer = question.DescriptionHtml != null ? question.DescriptionHtml : "",
            categories = question.Categories.Select(c => new
            {
                name = c.Name,
                categoryType = c.Type,
                linkToCategory = Links.CategoryDetail(c),
            }).AsEnumerable().Distinct().ToList(),
            references = question.References.Select(r => new
            {
                referenceType = r.ReferenceType.GetName(),
                additionalInfo = r.AdditionalInfo ?? "",
                referenceText = r.ReferenceText ?? ""
            }).AsEnumerable().Distinct().ToList(),
            author = author.Name,
            authorId = author.Id,
            authorImage = authorImage.Url,
            authorUrl = Links.UserDetail(author),
            extendedQuestion = question.TextExtendedHtml != null ? question.TextExtendedHtml : "",
            commentCount = Resolve<CommentRepository>().GetForDisplay(question.Id)
                .Where(c => !c.IsSettled)
                .Select(c => new CommentModel(c))
                .ToList()
                .Count(),
            isCreator = author.Id == _sessionUser.UserId,
            editUrl = Links.EditQuestion(Url, question.Text, question.Id),
            historyUrl = Links.QuestionHistory(question.Id),
            answerCount = history.TimesAnsweredUser,
            correctAnswerCount = history.TimesAnsweredUserTrue,
            wrongAnswerCount = history.TimesAnsweredUserWrong,
            canBeEdited = (question.Creator == _sessionUser.User) || _sessionUser.IsInstallationAdmin,
        });

        return json;
    }

    [HttpPost]
    public string RenderWishknowledgePinButton(bool isInWishknowledge)
    {
        return ViewRenderer.RenderPartialView("~/Views/Shared/AddToWishknowledgeButtonQuestionDetail.ascx", new AddToWishknowledge(isInWishknowledge, true), ControllerContext);
    }

    [HttpPost]
    public JsonResult GetUpdatedCorrectnessProbability(int questionId)
    {
        var question = EntityCache.GetQuestionById(questionId);
        var hasPersonalAnswer = false;
        var model = new AnswerQuestionModel(question, true);
        if (_sessionUser.IsLoggedIn)
        {
            ConcurrentDictionary<int, QuestionValuationCacheItem> userQuestionValuation = new ConcurrentDictionary<int, QuestionValuationCacheItem>();
            userQuestionValuation = UserCache.GetItem(_sessionUser.UserId).QuestionValuations;
            if (userQuestionValuation.ContainsKey(questionId))
                hasPersonalAnswer = userQuestionValuation[questionId].CorrectnessProbabilityAnswerCount > 0;
        }

        return Json(new
        {
            correctnessProbability = model.HistoryAndProbability.CorrectnessProbability.CPPersonal,
            hasPersonalAnswer
        });
    }
}