using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TrueOrFalse.Web.Uris;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse;

public class QuestionRowModel : BaseModel
{
    public string ImageUrl;
    public string CreatorName { get; private set; }
    public string QuestionShort { get; private set; }
    public int QuestionId { get; private set; }
    public int IndexInResulSet { get; private set; }

    public string CreatorUrlName { get; private set; }
    public int CreatorId { get; private set; }

    public bool IsPrivate;

    public bool IsOwner;
    public string TotalRelevancePersonalEntries;
    public string TotalRelevancePersonalAvg;

    public string TotalQualityEntries;
    public string TotalQualityAvg;
    public int RelevancePersonal;
    public int Views;

    public Func<UrlHelper, string> AnswerQuestionLink { get; private set; }
    public Func<UrlHelper, string> UserLink { get; private set;  }

    public IList<Category> Categories;
    public IList<SetMini> SetMinis;
    public int SetCount;

    public DateTime DateCreated;

    public HistoryAndProbabilityModel HistoryAndProbability;

    public QuestionRowModel(
        Question question, 
        TotalPerUser totalForUser, 
        QuestionValuation questionValuation,
        int indexInResultSet, 
        int currentUserid,
        string pagerKey) 
    {

        ImageUrl = QuestionImageSettings.Create(question.Id).GetUrl_128px_square().Url;

        QuestionShort = question.GetShortTitle();
        QuestionId = question.Id;
        CreatorName = question.Creator.Name;

        CreatorUrlName = UriSegmentFriendlyUser.Run(question.Creator.Name);
        CreatorId = question.Creator.Id;

        AnswerQuestionLink = urlHelper => Links.AnswerQuestion(urlHelper, question, indexInResultSet, pagerKey);
        UserLink = urlHelper => Links.UserDetail(urlHelper, question.Creator.Name, question.Creator.Id);

        RelevancePersonal = questionValuation.RelevancePersonal;
        
        IndexInResulSet = indexInResultSet;

        IsOwner = currentUserid == CreatorId;

        IsPrivate = question.Visibility != QuestionVisibility.All;

        TotalRelevancePersonalEntries = question.TotalRelevancePersonalEntries.ToString();
        TotalRelevancePersonalAvg = (question.TotalRelevancePersonalAvg / 10d).ToString();

        TotalQualityEntries = question.TotalQualityEntries.ToString();
        TotalQualityAvg = (question.TotalQualityAvg / 10d).ToString();

        Views = question.TotalViews;

        Categories = question.Categories;
        SetCount = question.SetsAmount;
        SetMinis = question.SetTop5Minis;

        var questionValuationForUser = NotNull.Run(Resolve<QuestionValuationRepository>().GetBy(question.Id, UserId));

        if (totalForUser == null)
            totalForUser = new TotalPerUser();

        HistoryAndProbability = new HistoryAndProbabilityModel
        {
            AnswerHistory = new AnswerHistoryModel(question, totalForUser),
            CorrectnessProbability = new CorrectnessProbabilityModel(question, questionValuationForUser)
        };

        DateCreated = question.DateCreated;
    }
}