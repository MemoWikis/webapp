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

    public int QuestionId { get; private set; }
    public string QuestionText { get; private set; }
    public string QuestionShort { get; private set; }
    public string CreatorName { get; private set; }
    public int IndexInResulSet { get; private set; }
    private readonly UserTinyModel Creator;
    public string CreatorUrlName { get; private set; }
    public int CreatorId { get; private set; }

    public bool IsPrivate;

    public bool IsCreator;
    public string TotalRelevancePersonalEntries;
    public string TotalRelevancePersonalAvg;

    public string TotalQualityEntries;
    public string TotalQualityAvg;
    public int Views;

    public Func<UrlHelper, string> AnswerQuestionLink { get; private set; }
    public Func<UrlHelper, string> UserLink { get; private set;  }

    public IList<SetMini> SetMinis;
    public int SetCount;

    public DateTime DateCreated;

    public HistoryAndProbabilityModel HistoryAndProbability;

    public bool IsInWishknowledge;

    public QuestionRowModel(
        Question question, 
        TotalPerUser totalForUser, 
        QuestionValuation questionValuation,
        int indexInResultSet, 
        SearchTabType searchTab) 
    {
        Creator = new UserTinyModel(question.Creator);
        ImageFrontendData = GetQuestionImageFrontendData.Run(question);

        Question = question;
        QuestionId = question.Id;
        QuestionText = question.Text;
        QuestionShort = question.GetShortTitle();
        CreatorName = Creator.Name;

        CreatorUrlName = UriSegmentFriendlyUser.Run(Creator.Name);
        CreatorId = Creator.Id;

        AnswerQuestionLink = urlHelper => Links.AnswerQuestion(question, indexInResultSet, searchTab.ToString());
        UserLink = urlHelper => Links.UserDetail(Creator.Name, Creator.Id);
        
        IndexInResulSet = indexInResultSet;

        IsCreator = UserId == CreatorId;

        IsPrivate = question.Visibility != QuestionVisibility.All;

        TotalRelevancePersonalEntries = question.TotalRelevancePersonalEntries.ToString();
        TotalRelevancePersonalAvg = (question.TotalRelevancePersonalAvg / 10d).ToString();

        TotalQualityEntries = question.TotalQualityEntries.ToString();
        TotalQualityAvg = (question.TotalQualityAvg / 10d).ToString();

        Views = question.TotalViews;

        SetCount = question.SetsAmount;
        SetMinis = question.SetTop5Minis;

        IsInWishknowledge = questionValuation.IsInWishKnowledge();

        HistoryAndProbability = new HistoryAndProbabilityModel
        {
            AnswerHistory = new AnswerHistoryModel(question, totalForUser),
            CorrectnessProbability = new CorrectnessProbabilityModel(question, questionValuation),
            QuestionValuation = questionValuation
        };

        DateCreated = question.DateCreated;
    }
}