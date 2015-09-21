﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web.Uris;

public class QuestionRowModel : BaseModel
{
    public Question Question;

    public ImageFrontendData ImageFrontendData;

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
        int currentUserid,
        SearchTabType searchTab) 
    {
        ImageFrontendData = GetQuestionImageFrontendData.Run(question);

        Question = question;
        QuestionShort = question.GetShortTitle();
        QuestionId = question.Id;
        CreatorName = question.Creator.Name;

        CreatorUrlName = UriSegmentFriendlyUser.Run(question.Creator.Name);
        CreatorId = question.Creator.Id;

        AnswerQuestionLink = urlHelper => Links.AnswerQuestion(urlHelper, question, indexInResultSet, searchTab.ToString());
        UserLink = urlHelper => Links.UserDetail(question.Creator.Name, question.Creator.Id);
        
        IndexInResulSet = indexInResultSet;

        IsOwner = currentUserid == CreatorId;

        IsPrivate = question.Visibility != QuestionVisibility.All;

        TotalRelevancePersonalEntries = question.TotalRelevancePersonalEntries.ToString();
        TotalRelevancePersonalAvg = (question.TotalRelevancePersonalAvg / 10d).ToString();

        TotalQualityEntries = question.TotalQualityEntries.ToString();
        TotalQualityAvg = (question.TotalQualityAvg / 10d).ToString();

        Views = question.TotalViews;

        SetCount = question.SetsAmount;
        SetMinis = question.SetTop5Minis;

        IsInWishknowledge = questionValuation.IsSetRelevancePersonal();

        HistoryAndProbability = new HistoryAndProbabilityModel
        {
            AnswerHistory = new AnswerHistoryModel(question, totalForUser),
            CorrectnessProbability = new CorrectnessProbabilityModel(question, questionValuation)
        };

        DateCreated = question.DateCreated;
    }
}