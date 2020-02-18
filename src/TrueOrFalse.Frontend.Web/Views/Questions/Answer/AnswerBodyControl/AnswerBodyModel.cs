﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

public class AnswerBodyModel : BaseModel
{
    public Guid QuestionViewGuid;
    public string CreationDate;
    public string CreationDateNiceText;

    public int QuestionId;

    public UserTinyModel Creator;
    public bool IsCreator;
    public bool IsInWishknowledge;
    public KnowledgeStatus KnowledgeStatus;

    public string QuestionLastEditedOn;
    public string QuestionText;
    public string QuestionTextMarkdown;
    public LicenseQuestion LicenseQuestion;
    public bool IsLastQuestion = false;
    public Question Question;

    public bool HasSound => !string.IsNullOrEmpty(SoundUrl);
    public string SoundUrl;
    
    public string SolutionMetaDataJson;
    public SolutionMetadata SolutionMetadata;
    public string SolutionType;
    public int SolutionTypeInt;
    public QuestionSolution SolutionModel;

    public bool? IsMobileRequest;

    public bool IsInWidget;
    public bool IsForVideo;
    public bool IsLearningSession;
    public LearningSession LearningSession;
    public bool IsLastLearningStep = false;
    public bool IsTestSession;
    public int TestSessionProgessAfterAnswering;
    public bool IsInLearningTab;
    public bool IsInTestMode = false;

    public bool HasCategories;
    public string PrimaryCategoryName;
    public int PrimaryCategoryId;

    public bool ShowCommentLink => 
        CommentCount != -1 && 
        !IsLearningSession && !IsTestSession && !DisableCommentLink;

    public int CommentCount = -1;

    public bool DisableCommentLink;
    public bool DisableAddKnowledgeButton;

    public Func<UrlHelper, string> NextUrl;
    
    public Func<UrlHelper, string> AjaxUrl_SendAnswer { get; private set; }
    public Func<UrlHelper, string> AjaxUrl_GetSolution { get; private set; }
    public Func<UrlHelper, string> AjaxUrl_CountLastAnswerAsCorrect { get; private set; }
    public Func<UrlHelper, string> AjaxUrl_CountUnansweredAsCorrect { get; private set; }
    public Func<UrlHelper, string> AjaxUrl_TestSessionRegisterAnsweredQuestion { get; private set; }
    public Func<UrlHelper, string> AjaxUrl_LearningSessionAmendAfterShowSolution { get; private set; }

    public int TotalActivityPoints;

    public AnswerBodyModel(Question question, Game game, Player player, Round round)
    {
        QuestionViewGuid = Guid.NewGuid();
        
        R<SaveQuestionView>().Run(QuestionViewGuid, question, _sessionUser.User.Id, player, round);
        
        var questionValuationForUser = NotNull.Run(Resolve<QuestionValuationRepo>().GetByFromCache(question.Id, UserId));
        IsInWishknowledge = questionValuationForUser.IsInWishKnowledge();

        if (player != null) 
        {
            AjaxUrl_SendAnswer = url => Links.SendAnswer(url, question, game, player, round);
            AjaxUrl_GetSolution = url => Links.GetSolution(url, question, round);
        }
        else
        {
            AjaxUrl_SendAnswer = url => Links.SendAnswer(url, question);
            AjaxUrl_GetSolution = url => Links.GetSolution(url, question);
        }

        Init(question);
    }

    public AnswerBodyModel(AnswerQuestionModel answerQuestionModel, bool isInLearningTab = false, bool isInTestMode = false)
    {
        QuestionViewGuid = answerQuestionModel.QuestionViewGuid;

        IsInWishknowledge = answerQuestionModel.IsInWishknowledge;


        IsMobileRequest = answerQuestionModel.IsMobileDevice;

        IsInWidget = answerQuestionModel.IsInWidget;
        IsLearningSession = answerQuestionModel.IsLearningSession;
        LearningSession = answerQuestionModel.LearningSession;
        IsTestSession = answerQuestionModel.IsTestSession;
        IsInLearningTab = isInLearningTab;
        TestSessionProgessAfterAnswering = answerQuestionModel.TestSessionProgessAfterAnswering;

        NextUrl = answerQuestionModel.NextUrl;

        IsLastQuestion = !IsLearningSession && !answerQuestionModel.HasNextPage;

        if (answerQuestionModel.IsLearningSession)
        {
            AjaxUrl_SendAnswer = url => Links.SendAnswer(
                url, 
                answerQuestionModel.Question,
                answerQuestionModel.LearningSession,
                answerQuestionModel.LearningSessionStep);

            IsLastLearningStep = answerQuestionModel.IsLastLearningStep;
        }
        else
        {
            AjaxUrl_SendAnswer = url => Links.SendAnswer(url, answerQuestionModel.Question);
        }

        AjaxUrl_GetSolution = url => Links.GetSolution(url, answerQuestionModel.Question);

        CommentCount = answerQuestionModel.Comments.GetTotalCount();

        DisableCommentLink = answerQuestionModel.DisableCommentLink;
        DisableAddKnowledgeButton = answerQuestionModel.DisableAddKnowledgeButton;

        IsInTestMode = isInTestMode;

        Init(answerQuestionModel.Question);
    }

    private void Init(Question question)
    {
        QuestionId = question.Id;
        Creator =  new UserTinyModel(question.Creator);
        IsCreator = Creator.Id == UserId;
        HasCategories = question.Categories.Any();

        if (HasCategories)
        {
            PrimaryCategoryId = question.Categories.FirstOrDefault().Id;
            PrimaryCategoryName = question.Categories.FirstOrDefault().Name;
        }

        CreationDate = question.DateCreated.ToString("dd.MM.yyyy HH:mm:ss");
        CreationDateNiceText = DateTimeUtils.TimeElapsedAsText(question.DateCreated);
        QuestionLastEditedOn = DateTimeUtils.TimeElapsedAsText(question.DateModified);
        Question = question;


        var questionValuationForUser = NotNull.Run(Sl.QuestionValuationRepo.GetByFromCache(question.Id, UserId));
        KnowledgeStatus = questionValuationForUser.KnowledgeStatus; 

        AjaxUrl_CountLastAnswerAsCorrect = url => Links.CountLastAnswerAsCorrect(url, question);
        AjaxUrl_CountUnansweredAsCorrect = url => Links.CountUnansweredAsCorrect(url, question);
        if (IsTestSession)
            AjaxUrl_TestSessionRegisterAnsweredQuestion = Links.TestSessionRegisterQuestionAnswered;

        if (IsLearningSession)
            AjaxUrl_LearningSessionAmendAfterShowSolution = Links.LearningSessionAmendAfterShowSolution;

        QuestionText = question.Text;
        QuestionTextMarkdown = question.TextExtended != null ? MarkdownMarkdig.ToHtml(question.TextExtended) : "";

        if (question.SolutionType == TrueOrFalse.SolutionType.FlashCard)
        {
            QuestionText = EscapeFlashCardText(QuestionText);
            QuestionTextMarkdown = EscapeFlashCardText(QuestionTextMarkdown);
        }

        LicenseQuestion = question.License;
                          
        SoundUrl = new GetQuestionSoundUrl().Run(question);

        SolutionMetadata = new SolutionMetadata { Json = question.SolutionMetadataJson };
        SolutionMetaDataJson = question.SolutionMetadataJson;
        SolutionType = question.SolutionType.ToString();
        SolutionTypeInt = (int)question.SolutionType;
        SolutionModel = GetQuestionSolution.Run(question);

        TotalActivityPoints = IsLoggedIn ? Sl.SessionUser.User.ActivityPoints : Sl.R<SessionUser>().getTotalActivityPoints();
    }

    private string EscapeFlashCardText(string text)
    {
        return text
            .Replace("'", "\\'")
            .Replace("\"", "\\\"")
            .Replace(Environment.NewLine, String.Empty)
            .Replace("\n", String.Empty);
    }
}