using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TrueOrFalse;

public class AnswerQuestionModel
{
    private readonly int _sessionUserId;
    private readonly TotalsPersUserLoader _totalsPersUserLoader;
    private readonly QuestionValuationRepo _questionValuationRepo;

    public AnswerQuestionModel(int sessionUserId, 
        TotalsPersUserLoader totalsPersUserLoader,
        QuestionValuationRepo questionValuationRepo)
    {
        _sessionUserId = sessionUserId;
        _totalsPersUserLoader = totalsPersUserLoader;
        _questionValuationRepo = questionValuationRepo;
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
        var valuationForUser = _totalsPersUserLoader.Run(_sessionUserId, question.Id);
        var questionValuationForUser = NotNull.Run(_questionValuationRepo.GetByFromCache(question.Id, _sessionUserId));

        HistoryAndProbability = new HistoryAndProbabilityModel
        {
            AnswerHistory = new AnswerHistoryModel(question, valuationForUser),
            CorrectnessProbability = new CorrectnessProbabilityModel(question, questionValuationForUser),
            QuestionValuation = questionValuationForUser
        };
    }
}
