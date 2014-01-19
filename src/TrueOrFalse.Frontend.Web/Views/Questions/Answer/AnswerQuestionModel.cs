﻿using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Web.Mvc;
using Gibraltar.Agent.Configuration;
using MarkdownSharp;
using Seedworks.Lib.Persistence;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Frontend.Web.Models;
using TrueOrFalse.Web;

public class AnswerQuestionModel : BaseModel
{
    public Func<UrlHelper, string> PreviousUrl;
    public Func<UrlHelper, string> NextUrl;

    public string QuestionId;
    public User Creator;
    public string CreatorId { get; private set; }
    public string CreatorName { get; private set; }

    public string PageCurrent;
    public string PagesTotal;
    public string PagerKey;
    public string PagerKeyOverviewPage;

    public string TotalQualityAvg;
    public string TotalQualityEntries;
    public string TotalRelevanceForAllAvg;
    public string TotalRelevanceForAllEntries;
    public string TotalRelevancePersonalAvg;
    public string TotalRelevancePersonalEntries;
    
    public string SolutionType;
    public QuestionSolution SolutionModel;
    public SolutionMetadata SolutionMetadata;
    public string SolutionMetaDataJson;

    public string ImageUrl_500px;
    public string SoundUrl;
    public IList<FeedbackRowModel> FeedbackRows;
    public int TotalViews;

    public int TimesAnsweredUser;
    public int TimesAnsweredUserTrue;
    public int TimesAnsweredUserWrong;

    public bool HasImage
    {
        get { return !string.IsNullOrEmpty(ImageUrl_500px); }
    }

    public bool HasSound
    {
        get { return !string.IsNullOrEmpty(SoundUrl); }
    }

    public string CreationDateNiceText { get; private set; }
    public string CreationDate { get; private set; }

    public int TimesAnsweredTotal { get; private set; }
    public int PercenctageCorrectAnswers { get; private set; }
    public int TimesAnsweredCorrect { get; private set; }
    public int TimesAnsweredWrongTotal { get; private set; }
    public int TimesJumpedOver { get; private set; }

    public string AverageAnswerTime { get; private set; }

    public string QuestionText { get; private set; }
    public string QuestionTextMarkdown { get; private set; }

    public Func<UrlHelper, string> AjaxUrl_SendAnswer { get; private set; }
    public Func<UrlHelper, string> AjaxUrl_GetAnswer { get; private set; }

    public bool HasPreviousPage;
    public bool HasNextPage;

    public bool SourceIsTabAll;
    public bool SourceIsTabMine;
    public bool SourceIsTabWish;

    public bool SourceIsSet;

    public Set Set;
    public IList<Category> Categories;
    public IList<SetMini> SetMinis;
    public int SetCount;
    
    public AnswerHistoryModel AnswerHistory;
    public CorrectnessProbabilityModel CorrectnessProbability;

    public AnswerQuestionModel() { }

    public AnswerQuestionModel(Question question, QuestionSearchSpec searchSpec) : this()
    {
        PageCurrent = searchSpec.CurrentPage.ToString();
        PagesTotal = searchSpec.PageCount.ToString();
        PagerKey = searchSpec.Key;
        PagerKeyOverviewPage = searchSpec.KeyOverviewPage;
        HasPreviousPage = searchSpec.HasPreviousPage();
        HasNextPage = searchSpec.HasNextPage();

        NextUrl = url => url.Action("Next", Links.AnswerQuestionController, new {pager = PagerKey});
        PreviousUrl = url => url.Action("Previous", Links.AnswerQuestionController, new {pager = PagerKey});

        SourceIsTabAll = QuestionSearchSpecSession.KeyPagerAll == searchSpec.KeyOverviewPage;
        SourceIsTabMine = QuestionSearchSpecSession.KeyPagerMine == searchSpec.KeyOverviewPage;
        SourceIsTabWish = QuestionSearchSpecSession.KeyPagerWish == searchSpec.KeyOverviewPage;

        Populate(question);
    }

    public AnswerQuestionModel(Set set, Question question) : this()
    {
        int pageCurrent = set.QuestionsInSet.GetIndex(question.Id) + 1;
        int pagesTotal = set.QuestionsInSet.Count;
        PageCurrent = pageCurrent.ToString();
        PagesTotal = pagesTotal.ToString();

        HasPreviousPage = pageCurrent > 1;
        HasNextPage = pageCurrent < pagesTotal;

        NextUrl = url => url.Action("Next", Links.AnswerQuestionController, new { setId = set.Id, questionId = question.Id });
        PreviousUrl = url => url.Action("Previous", Links.AnswerQuestionController, new { setId = set.Id, questionId = question.Id });

        SourceIsSet = true;
        Set = set;

        Populate(question);
    }

    private void Populate(Question question)
    {
        var questionValuationForUser = NotNull.Run(Resolve<QuestionValuationRepository>().GetBy(question.Id, _sessionUser.User.Id));
        var valuationForUser = Resolve<TotalsPersUserLoader>().Run(_sessionUser.User.Id, question.Id);

        Creator = question.Creator;
        CreatorId = question.Creator.Id.ToString();
        CreatorName = question.Creator.Name;
        CreationDate = question.DateCreated.ToString("dd.MM.yyyy HH:mm:ss");
        CreationDateNiceText = TimeElapsedAsText.Run(question.DateCreated);

        QuestionId = question.Id.ToString();
        QuestionText = question.Text;
        QuestionTextMarkdown = MardownInit.Run().Transform(question.TextExtended);
        SolutionType = question.SolutionType.ToString();
        SolutionModel = new GetQuestionSolution().Run(question);

        SolutionMetadata = new SolutionMetadata {Json = question.SolutionMetadataJson};
        SolutionMetaDataJson = question.SolutionMetadataJson;

        AnswerHistory = new AnswerHistoryModel(question, valuationForUser);
        CorrectnessProbability = new CorrectnessProbabilityModel(question, questionValuationForUser);
        
        TotalViews = question.TotalViews + 1;

        TotalQualityAvg = question.TotalQualityAvg.ToString();
        TotalQualityEntries = question.TotalQualityEntries.ToString();
        TotalRelevanceForAllAvg = question.TotalRelevanceForAllAvg.ToString();
        TotalRelevanceForAllEntries = question.TotalRelevanceForAllEntries.ToString();
        TotalRelevancePersonalAvg = question.TotalRelevancePersonalAvg.ToString();
        TotalRelevancePersonalEntries = question.TotalRelevancePersonalEntries.ToString();

        AverageAnswerTime = "";

        AjaxUrl_SendAnswer = url => Links.SendAnswer(url, question);
        AjaxUrl_GetAnswer = url => Links.GetAnswer(url, question);

        ImageUrl_500px = QuestionImageSettings.Create(question.Id).GetUrl_128px().Url;
        SoundUrl = new GetQuestionSoundUrl().Run(question);

        Categories = question.Categories;
        SetMinis = question.SetTop5Minis;
        SetCount = question.SetsAmount;

        FeedbackRows = new List<FeedbackRowModel>();
        FeedbackRows.Add(new FeedbackRowModel{
            Key = "RelevancePersonal",
            Title = "Merken. [UhrIcon]",
            FeedbackAverage = Math.Round(question.TotalRelevancePersonalAvg / 10d, 1).ToString(),
            FeedbackCount = question.TotalRelevancePersonalEntries.ToString(),
            HasUserValue = questionValuationForUser.IsSetRelevancePersonal(),
            UserValue = questionValuationForUser.RelevancePersonal.ToString()
        });
        FeedbackRows.Add(new FeedbackRowModel{
            Key = "Quality",
            Title = "Qualität",
            FeedbackAverage = Math.Round(question.TotalQualityAvg / 10d, 1).ToString(),
            FeedbackCount = question.TotalQualityEntries.ToString(),
            HasUserValue = questionValuationForUser.IsSetQuality(),
            UserValue = questionValuationForUser.Quality.ToString()
        });
        FeedbackRows.Add(new FeedbackRowModel{
            Key = "RelevanceForAll",
            Title = "Allgemeinwissen?",
            FeedbackAverage = Math.Round(question.TotalRelevanceForAllAvg / 10d, 1).ToString(),
            FeedbackCount = question.TotalRelevanceForAllEntries.ToString(),
            HasUserValue = questionValuationForUser.IsSetRelevanceForAll(),
            UserValue = questionValuationForUser.RelevanceForAll.ToString()
        });
    }
}
