using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TrueOrFalse.Web.Uris;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse;

public class QuestionRowModel
{
    public string CreatorName { get; private set; }

    public string QuestionShort { get; private set; }
    public int QuestionId { get; private set; }
    public int IndexInResulSet { get; private set; }

    public string CreatorUrlName { get; private set; }
    public int CreatorId { get; private set; }

    public int AnswersAllCount { get; private set; }
    public int AnswersAllPercentageTrue;
    public int AnswersAllPercentageFalse;

    public int AnswerMeCount { get; private set; }
    public int AnswerMePercentageTrue;
    public int AnswerMePercentageFalse;

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

    public QuestionRowModel(
        Question question, 
        TotalPerUser totalForUser, 
        QuestionValuation questionValuation,
        int indexInResultSet, 
        int currentUserid,
        string pagerKey) 
    {
        QuestionShort = question.GetShortTitle();
        QuestionId = question.Id;
        CreatorName = question.Creator.Name;

        CreatorUrlName = UriSegmentFriendlyUser.Run(question.Creator.Name);
        CreatorId = question.Creator.Id;

        AnswerQuestionLink = urlHelper => Links.AnswerQuestion(urlHelper, question, indexInResultSet, pagerKey);
        UserLink = urlHelper => Links.UserDetail(urlHelper, question.Creator.Name, question.Creator.Id);

        AnswersAllCount = question.TotalAnswers();
        AnswersAllPercentageTrue = question.TotalTrueAnswersPercentage();
        AnswersAllPercentageFalse = question.TotalFalseAnswerPercentage();

        if(totalForUser == null)
            totalForUser = new TotalPerUser();

        AnswerMeCount = totalForUser.Total();
        AnswerMePercentageTrue = totalForUser.PercentageTrue();
        AnswerMePercentageFalse = totalForUser.PercentageFalse();

        RelevancePersonal = questionValuation.RelevancePersonal;
        
        IndexInResulSet = indexInResultSet;

        IsOwner = currentUserid == CreatorId;

        TotalRelevancePersonalEntries = question.TotalRelevancePersonalEntries.ToString();
        TotalRelevancePersonalAvg = (question.TotalRelevancePersonalAvg / 10d).ToString();

        TotalQualityEntries = question.TotalQualityEntries.ToString();
        TotalQualityAvg = (question.TotalQualityAvg / 10d).ToString();

        Views = question.TotalViews;

        Categories = question.Categories;
        SetCount = question.SetsAmount;
        SetMinis = question.SetTop5Minis;
    }
}