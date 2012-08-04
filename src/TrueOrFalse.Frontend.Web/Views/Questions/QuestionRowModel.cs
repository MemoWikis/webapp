using System;
using System.Web.Mvc;
using TrueOrFalse.Core.Web.Uris;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Core;

public class QuestionRowModel
{

    public string CreatorName { get; private set; }

    public string QuestionShort { get; private set; }
    public int QuestionId { get; private set; }
    public int IndexInresulSet { get; private set; }

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
    public Func<UrlHelper, string> UserProfileLink { get; private set;  }

    public QuestionRowModel(Question question, 
                            TotalPerUser totalForUser, 
                            QuestionValuation questionValuation,
                            int indexInResultSet, 
                            int currentUserid) 
    {
        QuestionShort = question.GetShortTitle();
        QuestionId = question.Id;
        CreatorName = question.Creator.Name;

        CreatorUrlName = UriSegmentFriendlyUser.Run(question.Creator.Name);
        CreatorId = question.Creator.Id;

        AnswerQuestionLink = url => Links.AnswerQuestion(url, question, indexInResultSet);
        UserProfileLink = url => Links.Profile(url, question.Creator.Name, question.Creator.Id);

        AnswersAllCount = question.TotalAnswers();
        AnswersAllPercentageTrue = question.TotalTrueAnswersPercentage();
        AnswersAllPercentageFalse = question.TotalFalseAnswerPercentage();

        if(totalForUser == null)
            totalForUser = new TotalPerUser();

        AnswerMeCount = totalForUser.Total();
        AnswerMePercentageTrue = totalForUser.PercentageTrue();
        AnswerMePercentageFalse = totalForUser.PercentageFalse();

        RelevancePersonal = questionValuation.RelevancePersonal;
        
        IndexInresulSet = indexInResultSet;

        IsOwner = currentUserid == CreatorId;

        TotalRelevancePersonalEntries = question.TotalRelevancePersonalEntries.ToString();
        TotalRelevancePersonalAvg = (question.TotalRelevancePersonalAvg / 10d).ToString();

        TotalQualityEntries = question.TotalQualityEntries.ToString();
        TotalQualityAvg = (question.TotalQualityAvg / 10d).ToString();

        Views = question.TotalViews;
    }
}