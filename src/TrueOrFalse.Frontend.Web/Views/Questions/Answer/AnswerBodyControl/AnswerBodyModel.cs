﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

public class AnswerBodyModel : BaseModel
{
    public Guid QuestionViewGuid;

    public int QuestionId;
    public User Creator;
    public bool IsInWishknowledge;

    public string QuestionText;
    public string QuestionTextMarkdown;

    public LicenseQuestion LicenseQuestion;

    public bool HasSound{ get { return !string.IsNullOrEmpty(SoundUrl); } }
    public string SoundUrl;
    
    public string SolutionMetaDataJson;
    public SolutionMetadata SolutionMetadata;
    public string SolutionType;
    public int SolutionTypeInt;
    public QuestionSolution SolutionModel;

    public bool IsLearningSession;
    public LearningSession LearningSession;
    public bool IsLastLearningStep = false;
    public bool IsTestSession;
    public int TestSessionProgessAfterAnswering;

    public bool IsLastQuestion = false;

    public bool ShowCommentLink => CommentCount != -1 && !IsLearningSession && !IsTestSession;
    public int CommentCount = -1;

    public Func<UrlHelper, string> NextUrl;
    
    public Func<UrlHelper, string> AjaxUrl_SendAnswer { get; private set; }
    public Func<UrlHelper, string> AjaxUrl_GetSolution { get; private set; }
    public Func<UrlHelper, string> AjaxUrl_CountLastAnswerAsCorrect { get; private set; }
    public Func<UrlHelper, string> AjaxUrl_CountUnansweredAsCorrect { get; private set; }
    public Func<UrlHelper, string> AjaxUrl_TestSessionRegisterAnsweredQuestion { get; private set; }

    public AnswerBodyModel(Question question, Game game, Player player, Round round)
    {
        QuestionViewGuid = Guid.NewGuid();

        R<SaveQuestionView>().Run(QuestionViewGuid, question, _sessionUser.User.Id, player, round);
        
        var questionValuationForUser = NotNull.Run(Resolve<QuestionValuationRepo>().GetBy(question.Id, UserId));
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

    public AnswerBodyModel(AnswerQuestionModel answerQuestionModel)
    {
        QuestionViewGuid = answerQuestionModel.QuestionViewGuid;

        IsInWishknowledge = answerQuestionModel.IsInWishknowledge;

        IsLearningSession = answerQuestionModel.IsLearningSession;
        LearningSession = answerQuestionModel.LearningSession;
        IsTestSession = answerQuestionModel.IsTestSession;
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

        Init(answerQuestionModel.Question);
    }

    private void Init(Question question)
    {
        QuestionId = question.Id;
        Creator = question.Creator;

        AjaxUrl_CountLastAnswerAsCorrect = url => Links.CountLastAnswerAsCorrect(url, question);
        AjaxUrl_CountUnansweredAsCorrect = url => Links.CountUnansweredAsCorrect(url, question);
        if (IsTestSession)
            AjaxUrl_TestSessionRegisterAnsweredQuestion = Links.TestSessionRegisterQuestionAnswered;

        QuestionText = question.Text;
        QuestionTextMarkdown = MarkdownInit.Run().Transform(question.TextExtended);
        LicenseQuestion = question.License;
                          
        SoundUrl = new GetQuestionSoundUrl().Run(question);

        SolutionMetadata = new SolutionMetadata { Json = question.SolutionMetadataJson };
        SolutionMetaDataJson = question.SolutionMetadataJson;
        SolutionType = question.SolutionType.ToString();
        SolutionTypeInt = (int)question.SolutionType;
        SolutionModel = GetQuestionSolution.Run(question);
    }
}