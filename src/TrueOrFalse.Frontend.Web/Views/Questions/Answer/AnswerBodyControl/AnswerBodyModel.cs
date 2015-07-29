﻿using System;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

public class AnswerBodyModel : BaseModel
{
    public int QuestionId;
    public bool IsInWishknowledge;

    public string QuestionText;
    public string QuestionTextMarkdown;

    public bool HasSound{ get { return !string.IsNullOrEmpty(SoundUrl); } }
    public string SoundUrl;
    
    public string SolutionMetaDataJson;
    public SolutionMetadata SolutionMetadata;
    public string SolutionType;
    public int SolutionTypeInt;
    public QuestionSolution SolutionModel;

    public Func<UrlHelper, string> NextUrl;
    public Func<UrlHelper, string> AjaxUrl_SendAnswer { get; private set; }
    public Func<UrlHelper, string> AjaxUrl_GetSolution { get; private set; }
    public Func<UrlHelper, string> AjaxUrl_CountLastAnswerAsCorrect { get; private set; }

    public AnswerBodyModel(Question question, Game game, Player player, Round round)
    {
        var questionValuationForUser = NotNull.Run(Resolve<QuestionValuationRepo>().GetBy(question.Id, UserId));
        IsInWishknowledge = questionValuationForUser.IsSetRelevancePersonal();

        AjaxUrl_SendAnswer = url => Links.SendAnswer(url, question, game, player, round);

        Init(question);
    }

    public AnswerBodyModel(AnswerQuestionModel answerQuestionModel)
    {
        IsInWishknowledge = answerQuestionModel.IsInWishknowledge;
        
        NextUrl = answerQuestionModel.NextUrl;

        if (answerQuestionModel.IsLearningSession)
        {
            AjaxUrl_SendAnswer = url => Links.SendAnswer(
                url, 
                answerQuestionModel.Question, 
                answerQuestionModel.LearningSessionStep);
        }
        else
        {
            AjaxUrl_SendAnswer = url => Links.SendAnswer(url, answerQuestionModel.Question);
        }

        Init(answerQuestionModel.Question);
    }

    private void Init(Question question)
    {
        QuestionId = question.Id;

        AjaxUrl_GetSolution = url => Links.GetAnswer(url, question);
        AjaxUrl_CountLastAnswerAsCorrect = url => Links.CountLastAnswerAsCorrect(url, question);

        QuestionText = question.Text;
        QuestionTextMarkdown = MardownInit.Run().Transform(question.TextExtended);

        SoundUrl = new GetQuestionSoundUrl().Run(question);

        SolutionMetadata = new SolutionMetadata { Json = question.SolutionMetadataJson };
        SolutionMetaDataJson = question.SolutionMetadataJson;
        SolutionType = question.SolutionType.ToString();
        SolutionTypeInt = (int)question.SolutionType;
        SolutionModel = new GetQuestionSolution().Run(question);
    }
}