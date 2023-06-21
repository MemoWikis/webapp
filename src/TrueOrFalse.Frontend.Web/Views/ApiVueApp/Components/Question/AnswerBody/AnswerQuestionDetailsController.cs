using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class AnswerQuestionDetailsController: BaseController
{
    [HttpGet]
    public JsonResult Get(int id) => Json(GetData(id), JsonRequestBehavior.AllowGet);

    public dynamic GetData(int id)
    {
        var question = EntityCache.GetQuestionById(id);

        if (question.Id == 0 || !PermissionCheck.CanView(question))
            return Json(null);

        var dateNow = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var answerQuestionModel = new AnswerQuestionModel(question, true);
        var correctnessProbability = answerQuestionModel.HistoryAndProbability.CorrectnessProbability;
        var history = answerQuestionModel.HistoryAndProbability.AnswerHistory;

        var userQuestionValuation = SessionUser.IsLoggedIn
            ? SessionUserCache.GetItem(SessionUser.UserId).QuestionValuations
            : new ConcurrentDictionary<int, QuestionValuationCacheItem>();
        var hasUserValuation = userQuestionValuation.ContainsKey(question.Id) && SessionUser.IsLoggedIn;

        return new {
            knowledgeStatus = hasUserValuation ? userQuestionValuation[question.Id].KnowledgeStatus : KnowledgeStatus.NotLearned,
            personalProbability = correctnessProbability.CPPersonal,
            personalColor = correctnessProbability.CPPColor,
            avgProbability = correctnessProbability.CPAll,
            personalAnswerCount = history.TimesAnsweredUser,
            personalAnsweredCorrectly = history.TimesAnsweredUserTrue,
            personalAnsweredWrongly = history.TimesAnsweredUserWrong,
            overallAnswerCount = history.TimesAnsweredTotal,
            overallAnsweredCorrectly = history.TimesAnsweredCorrect,
            overallAnsweredWrongly = history.TimesAnsweredWrongTotal,
            isInWishknowledge = answerQuestionModel.HistoryAndProbability.QuestionValuation.IsInWishKnowledge,
            topics = question.CategoriesVisibleToCurrentUser().Select(t => new
            {
                Id = t.Id,
                Name = t.Name,
                Url = Links.CategoryDetail(t.Name, t.Id),
                QuestionCount = t.GetCountQuestionsAggregated(),
                ImageUrl = new CategoryImageSettings(t.Id).GetUrl_128px(asSquare: true).Url,
                IconHtml = SearchApiController.GetIconHtml(t),
                MiniImageUrl = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(t.Id, ImageType.Category))
                    .GetImageUrl(30, true, false, ImageType.Category).Url,
                Visibility = (int)t.Visibility,
                IsSpoiler = IsSpoilerCategory.Yes(t.Name, question)
            }).Distinct().ToArray(),

            visibility = question.Visibility,
            dateNow,
            endTimer = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            creator = new
            {
                id = question.CreatorId,
                name = question.Creator.Name
            },
            creationDate = DateTimeUtils.TimeElapsedAsText(question.DateCreated),
            totalViewCount = question.TotalViews,
            wishknowledgeCount = question.TotalRelevancePersonalEntries,
            license = new
            {
                isDefault = question.License.IsDefault(),
                shortText = question.License.DisplayTextShort,
                fullText = question.License.DisplayTextFull
            }
        };

    }
}