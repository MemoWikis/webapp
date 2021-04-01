using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web.Uris;

public class QuestionRowModel : BaseModel
{
    public Question Question;

    public ImageFrontendData ImageFrontendData;

    public int QuestionId { get; }
    public string QuestionText { get; }
    public string QuestionShort { get; }
    public string CreatorName { get; }
    public int IndexInResulSet { get; }
    public string CreatorUrlName { get; }
    public int CreatorId { get; }

    public bool IsPrivate;

    public bool IsCreator;
    public string TotalRelevancePersonalEntries;
    public string TotalRelevancePersonalAvg;

    public string TotalQualityEntries;
    public string TotalQualityAvg;
    public int Views;

    public Func<UrlHelper, string> AnswerQuestionLink { get; }
    public Func<UrlHelper, string> UserLink { get; }

    public DateTime DateCreated;

    public HistoryAndProbabilityModel HistoryAndProbability;

    public bool IsInWishknowledge;

    public QuestionRowModel(
        Question question, 
        TotalPerUser totalForUser, 
        QuestionValuationCacheItem questionValuation,
        int indexInResultSet, 
        SearchTabType searchTab) 
    {
        var creator = new UserTinyModel(question.Creator);
        ImageFrontendData = GetQuestionImageFrontendData.Run(question);

        Question = question;
        QuestionId = question.Id;
        QuestionText = question.Text;
        QuestionShort = question.GetShortTitle();
        CreatorName = creator.Name;

        CreatorUrlName = UriSegmentFriendlyUser.Run(creator.Name);
        CreatorId = creator.Id;

        AnswerQuestionLink = urlHelper => Links.AnswerQuestion(question, indexInResultSet, searchTab.ToString());
        UserLink = urlHelper => Links.UserDetail(creator.Name, creator.Id);
        
        IndexInResulSet = indexInResultSet;

        IsCreator = UserId == CreatorId;

        IsPrivate = question.Visibility != QuestionVisibility.All;

        TotalRelevancePersonalEntries = question.TotalRelevancePersonalEntries.ToString();
        TotalRelevancePersonalAvg = (question.TotalRelevancePersonalAvg / 10d).ToString();

        TotalQualityEntries = question.TotalQualityEntries.ToString();
        TotalQualityAvg = (question.TotalQualityAvg / 10d).ToString();

        Views = question.TotalViews;

        IsInWishknowledge = questionValuation.IsInWishKnowledge;

        HistoryAndProbability = new HistoryAndProbabilityModel
        {
            AnswerHistory = new AnswerHistoryModel(question, totalForUser),
            CorrectnessProbability = new CorrectnessProbabilityModel(question, questionValuation),
            QuestionValuation = questionValuation
        };

        DateCreated = question.DateCreated;
    }
}