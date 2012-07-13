using System;
using System.Web.Mvc;
using TrueOrFalse.Core.Web.Uris;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Core;

public class QuestionRowModel
{
    public int AnswerPercentageTrue;
    public int AnswerPercentageFalse;

    public QuestionRowModel(Question question, int indexInResultSet, int currentUserid) 
    {
        QuestionShort = question.GetShortTitle();
        QuestionId = question.Id;
        CreatorName = question.Creator.Name;

        CreatorUrlName = UriSegmentFriendlyUser.Run(question.Creator.Name);
        CreatorId = question.Creator.Id;

        AnswerQuestionLink = url => Links.AnswerQuestion(url, question, indexInResultSet);

        AnswerCountTotal = question.TotalAnswers();
        AnswerPercentageTrue = question.TotalTrueAnswersPercentage();
        AnswerPercentageFalse = question.TotalFalseAnswerPercentage();
        AnswerCountMe = 0;
        IndexInresulSet = indexInResultSet;

        IsOwner = currentUserid == CreatorId;

        TotalRelevancePersonalEntries = question.TotalRelevancePersonalEntries.ToString();
        TotalRelevancePersonalAvg = (question.TotalRelevancePersonalAvg / 10d).ToString();

        TotalQualityEntries = question.TotalQualityEntries.ToString();
        TotalQualityAvg = (question.TotalQualityAvg / 10d).ToString();

    }
    public string CreatorName {get; private set;}

    public string QuestionShort { get; private set; }
    public int QuestionId { get; private set; }
    public int IndexInresulSet { get; private set; }
   
    public string CreatorUrlName { get; private set; }
    public int CreatorId { get; private set; }

    public int AnswerCountTotal { get; private set; }
    public int AnswerCountMe { get; private set; }

    public bool IsOwner;
    public string TotalRelevancePersonalEntries;
    public string TotalRelevancePersonalAvg;
    
    public string TotalQualityEntries;
    public string TotalQualityAvg;

    public Func<UrlHelper, string> AnswerQuestionLink { get; private set; }
}