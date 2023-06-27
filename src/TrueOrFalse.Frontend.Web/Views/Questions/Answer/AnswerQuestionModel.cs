using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TrueOrFalse;

public class AnswerQuestionModel :  BaseResolve
{
    private readonly int _sessionUserId;

    public AnswerQuestionModel(int sessionUserId)
    {
        _sessionUserId = sessionUserId;
    }

    public int QuestionId;
    public QuestionCacheItem Question;
    public UserTinyModel Creator;
    public string CreatorId { get; private set; }
    public string QuestionText { get; private set; }
    public QuestionVisibility Visibility { get; private set; }
    public IList<CategoryCacheItem> Categories;
    public HistoryAndProbabilityModel HistoryAndProbability;
    public LearningSession  LearningSession;
    public AnswerQuestionModel(QuestionCacheItem question, bool isQuestionDetails)
    {
        var valuationForUser = Resolve<TotalsPersUserLoader>().Run(_sessionUserId, question.Id);
        var questionValuationForUser = NotNull.Run(Sl.QuestionValuationRepo.GetByFromCache(question.Id, _sessionUserId));

        HistoryAndProbability = new HistoryAndProbabilityModel
        {
            AnswerHistory = new AnswerHistoryModel(question, valuationForUser),
            CorrectnessProbability = new CorrectnessProbabilityModel(question, questionValuationForUser),
            QuestionValuation = questionValuationForUser
        };
    }
}
