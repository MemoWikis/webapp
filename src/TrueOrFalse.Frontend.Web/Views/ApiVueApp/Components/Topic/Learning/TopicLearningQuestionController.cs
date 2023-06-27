using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class TopicLearningQuestionController: BaseController
{
    [HttpPost]
    public JsonResult LoadQuestionData(int questionId)
    {
        var question = EntityCache.GetQuestionById(questionId);
        var author = new UserTinyModel(question.Creator);
        var authorImage = new UserImageSettings(author.Id).GetUrl_128px_square(author);
        var solution = GetQuestionSolution.Run(question);
        var answerQuestionModel = new AnswerQuestionModel(question, true);
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
                commentCount = Resolve<CommentRepository>().GetForDisplay(question.Id)
                    .Where(c => !c.IsSettled)
                    .Select(c => new CommentModel(c))
                    .ToList()
                    .Count(),
                isCreator = author.Id == SessionUser.UserId,
                answerCount = history.TimesAnsweredUser,
                correctAnswerCount = history.TimesAnsweredUserTrue,
                wrongAnswerCount = history.TimesAnsweredUserWrong,
                canBeEdited = question.Creator?.Id == SessionUser.UserId || IsInstallationAdmin,
                title = question.Text,
                visibility = question.Visibility
            }
        });

        return json;
    }

    [HttpGet]
    public JsonResult GetKnowledgeStatus(int id)
    {
        var userQuestionValuation = SessionUser.IsLoggedIn
            ? SessionUserCache.GetItem(SessionUser.UserId).QuestionValuations
            : new ConcurrentDictionary<int, QuestionValuationCacheItem>();

        var hasUserValuation = userQuestionValuation.ContainsKey(id) && SessionUser.IsLoggedIn;

        return Json(hasUserValuation ? userQuestionValuation[id].KnowledgeStatus : KnowledgeStatus.NotLearned, JsonRequestBehavior.AllowGet);
    }
}