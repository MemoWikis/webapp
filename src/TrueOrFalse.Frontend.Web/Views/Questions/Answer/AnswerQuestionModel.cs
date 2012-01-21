using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Core;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Frontend.Web.Models;


public class AnswerQuestionModel : ModelBase
{
    public AnswerQuestionModel()
    {
        ShowLeftMenu_Nav();
        MainFullWidth = true;
    }

    public AnswerQuestionModel(Question question) : this()
    {
        CreatorId = question.Creator.Id.ToString();
        CreatorName = question.Creator.Name;
        CreationDate = question.DateCreated.ToString("yyyy.MM.dd");

        QuestionId = question.Id.ToString();
        QuestionText = question.Text;

        TimesAnswered = "0";
        PercenctageCorrectAnswers = "34";
        TimesAnsweredCorrect = "0";
        TimesAnsweredWrong = "";
        TimesJumpedOver = "";

        AverageAnswerTime = "";

        AnswerQuestionLink = url => Links.SendAnswer(url, question);

    }

    public string QuestionId;
    public string CreatorId { get; private set; }
    public string CreatorName { get; private set; }

    public string CreationDate { get; private set; }

    public string TimesAnswered { get; private set; }
    public string PercenctageCorrectAnswers { get; private set; }
    public string TimesAnsweredCorrect { get; private set; }
    public string TimesAnsweredWrong { get; private set; }
    public string TimesJumpedOver { get; private set; }

    public string AverageAnswerTime { get; private set; }

    public string QuestionText { get; private set; }

    public Func<UrlHelper, string> AnswerQuestionLink { get; private set; }
}