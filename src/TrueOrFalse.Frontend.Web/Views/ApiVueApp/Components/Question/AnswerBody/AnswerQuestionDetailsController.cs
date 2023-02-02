using System;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class AnswerQuestionDetailsController: BaseController
{
    [HttpGet]
    public JsonResult Get(int id)
    {
        var dateNow = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var question = EntityCache.GetQuestionById(id);
        var answerQuestionModel = new AnswerQuestionModel(question, true);
        var correctnessProbability = answerQuestionModel.HistoryAndProbability.CorrectnessProbability;
        var history = answerQuestionModel.HistoryAndProbability.AnswerHistory;
        var json = Json(new
        {
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
                Visibility = (int)t.Visibility
            }).Distinct().ToArray(),

            visibility = question.Visibility,
            dateNow,
            endTimer = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        }, JsonRequestBehavior.AllowGet);
        return json;
    }
}