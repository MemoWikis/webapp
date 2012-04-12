using System;
using System.Web.Mvc;
using TrueOrFalse.Core.Web.Uris;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Core;

public class QuestionRowModel
{
    public int AnswerPercentageTrue;
    public int AnswerPercentageFalse;

    public QuestionRowModel(Question question) 
    {
        QuestionShort = question.GetShortTitle();
        QuestionId = question.Id;
        CreatorName = question.Creator.Name;

        CreatorUrlName = UriSegmentFriendlyUser.Run(question.Creator.Name);
        CreatorId = question.Creator.Id;

        AnswerQuestionLink = url => Links.AnswerQuestion(url, question);

        AnswerCountTotal = question.TotalAnswers();
        AnswerPercentageTrue = question.TotalTrueAnswersPercentage();
        AnswerPercentageFalse = question.TotalFalseAnswerPercentage();
        AnswerCountMe = 0;

    }
    public string CreatorName {get; private set;}

    public string QuestionShort { get; private set; }
    public int QuestionId { get; private set; }

    public string CreatorUrlName { get; private set; }
    public int CreatorId { get; private set; }

    public int AnswerCountTotal { get; private set; }
    public int AnswerCountMe { get; private set; }

    public Func<UrlHelper, string> AnswerQuestionLink { get; private set; }
}