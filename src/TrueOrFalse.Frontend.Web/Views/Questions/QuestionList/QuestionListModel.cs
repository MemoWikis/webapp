using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Frontend.Web.Code;

public class QuestionListModel
{
    private SessionUser _sessionUser { get; }
    private readonly LearningSessionCache _learningSessionCache;
    private readonly CategoryValuationRepo _categoryValuationRepo;
    public int CategoryId;

    public QuestionListModel(LearningSessionCache learningSessionCache,
        SessionUser sessionUser,
        CategoryValuationRepo categoryValuationRepo)
    {
        _sessionUser = sessionUser;
        _learningSessionCache = learningSessionCache;
        _categoryValuationRepo = categoryValuationRepo;
    }

    public List<QuestionListJson.Question> PopulateQuestionsOnPage(int currentPage, int itemCountPerPage)
    {
        var learningSession = _learningSessionCache.GetLearningSession();

        var userQuestionValuation = _sessionUser.IsLoggedIn 
            ? SessionUserCache.GetItem(_sessionUser.UserId, _categoryValuationRepo).QuestionValuations 
            : new ConcurrentDictionary<int, QuestionValuationCacheItem>();

        var steps = learningSession.Steps;
        var stepsOfCurrentPage = steps.Skip(itemCountPerPage * (currentPage - 1)).Take(itemCountPerPage).ToList();
        stepsOfCurrentPage.RemoveAll(s => s.Question.Id == 0);

        var newQuestionList = new List<QuestionListJson.Question>();

        foreach (var step in stepsOfCurrentPage)
        {
            var q = step.Question;

            var hasUserValuation = userQuestionValuation.ContainsKey(q.Id) && _sessionUser.IsLoggedIn;
            var question = new QuestionListJson.Question
            {
                Id = q.Id,
                Title = q.Text,
                LinkToQuestion = Links.GetUrl(q),
                ImageData = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(q.Id, ImageType.Question)).GetImageUrl(40, true).Url,
                LearningSessionStepCount = steps.Count,
                LinkToQuestionVersions = Links.QuestionHistory(q.Id),
                LinkToComment = Links.GetUrl(q) + "#JumpLabel",
                CorrectnessProbability = q.CorrectnessProbability,
                Visibility = q.Visibility,
                SessionIndex = steps.IndexOf(step),
                KnowledgeStatus = hasUserValuation ? userQuestionValuation[q.Id].KnowledgeStatus : KnowledgeStatus.NotLearned,
            };

            if (userQuestionValuation.ContainsKey(q.Id) && _sessionUser.IsLoggedIn)
            {
                question.CorrectnessProbability = userQuestionValuation[q.Id].CorrectnessProbability;
                question.IsInWishknowledge = userQuestionValuation[q.Id].IsInWishKnowledge;
                question.HasPersonalAnswer = userQuestionValuation[q.Id].CorrectnessProbabilityAnswerCount > 0;
            }
            newQuestionList.Add(question);
        }

        return newQuestionList;
    }
}