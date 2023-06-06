using System.Collections.Concurrent;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class TopicLearningQuestionListController: BaseController
{

    public TopicLearningQuestionListController(SessionUser sessionUser) : base(sessionUser)
    {
        
    }
    [HttpPost]
    public JsonResult LoadQuestions(int itemCountPerPage, int pageNumber, int topicId)
    {
        if (LearningSessionCache.GetLearningSession() == null || topicId != LearningSessionCache.GetLearningSession().Config.CategoryId)
        {
            var config = new LearningSessionConfig
            {
                CategoryId = topicId,
                CurrentUserId = IsLoggedIn ? UserId : default
            };
            LearningSessionCache.AddOrUpdate(LearningSessionCreator.BuildLearningSession(config));
        }

        return Json(QuestionListModel.PopulateQuestionsOnPage(pageNumber, itemCountPerPage));
    }

    [HttpGet]
    public JsonResult LoadNewQuestion(int index)
    {
        var steps = LearningSessionCache.GetLearningSession().Steps;
        var question = steps[index].Question;

        var userQuestionValuation = SessionUserLegacy.IsLoggedIn
            ? SessionUserCache.GetItem(SessionUserLegacy.UserId).QuestionValuations
            : new ConcurrentDictionary<int, QuestionValuationCacheItem>();

        var hasUserValuation = userQuestionValuation.ContainsKey(question.Id) && SessionUserLegacy.IsLoggedIn;

        return Json( new {
            Id = question.Id,
            Title = question.Text,
            LinkToQuestion = Links.GetUrl(question),
            ImageData = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(question.Id, ImageType.Question)).GetImageUrl(40, true).Url,
            LearningSessionStepCount = steps.Count,
            LinkToQuestionVersions = Links.QuestionHistory(question.Id),
            LinkToComment = Links.GetUrl(question) + "#JumpLabel",
            CorrectnessProbability = hasUserValuation ? userQuestionValuation[question.Id].CorrectnessProbability : question.CorrectnessProbability,
            KnowledgeStatus = hasUserValuation ? userQuestionValuation[question.Id].KnowledgeStatus : KnowledgeStatus.NotLearned,
            Visibility = question.Visibility,
            SessionIndex = index,
            IsInWishknowledge = hasUserValuation && userQuestionValuation[question.Id].IsInWishKnowledge,
            HasPersonalAnswer = false
        }, JsonRequestBehavior.AllowGet);
    }
}