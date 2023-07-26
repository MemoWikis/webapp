using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Frontend.Web.Code;

public class QuestionListModel
{
    private SessionUser _sessionUser { get; }
    private readonly LearningSessionCache _learningSessionCache;
    private readonly CategoryValuationReadingRepo _categoryValuationReadingRepo;
    private readonly ImageMetaDataRepo _imageMetaDataRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly QuestionValuationRepo _questionValuationRepo;
    public int CategoryId;

    public QuestionListModel(LearningSessionCache learningSessionCache,
        SessionUser sessionUser,
        CategoryValuationReadingRepo categoryValuationReadingRepo,
        ImageMetaDataRepo imageMetaDataRepo,
        UserReadingRepo userReadingRepo,
        QuestionValuationRepo questionValuationRepo)
    {
        _sessionUser = sessionUser;
        _learningSessionCache = learningSessionCache;
        _categoryValuationReadingRepo = categoryValuationReadingRepo;
        _imageMetaDataRepo = imageMetaDataRepo;
        _userReadingRepo = userReadingRepo;
        _questionValuationRepo = questionValuationRepo;
    }

    public List<QuestionListJson.Question> PopulateQuestionsOnPage(int currentPage, int itemCountPerPage)
    {
        var learningSession = _learningSessionCache.GetLearningSession();

        var userQuestionValuation = _sessionUser.IsLoggedIn 
            ? SessionUserCache.GetItem(_sessionUser.UserId, _categoryValuationReadingRepo, _userReadingRepo, _questionValuationRepo).QuestionValuations 
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
                ImageData = new ImageFrontendData(_imageMetaDataRepo.GetBy(q.Id, ImageType.Question)).GetImageUrl(40, true).Url,
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