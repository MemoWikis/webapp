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
    public Func<UrlHelper, string> AjaxUrl_GetAnswer { get; private set; }
    public Func<UrlHelper, string> AjaxUrl_CountLastAnswerAsCorrect { get; private set; }

    public AnswerBodyModel(Question question)
    {
        var questionValuationForUser = NotNull.Run(Resolve<QuestionValuationRepository>().GetBy(question.Id, UserId));
        IsInWishknowledge = questionValuationForUser.IsSetRelevancePersonal();

        Init(question);
    }

    public AnswerBodyModel(AnswerQuestionModel answerQuestionModel)
    {
        IsInWishknowledge = answerQuestionModel.IsInWishknowledge;
        NextUrl = answerQuestionModel.NextUrl;

        Init(answerQuestionModel.Question);
    }

    private void Init(Question question)
    {
        QuestionId = question.Id;

        AjaxUrl_SendAnswer = url => Links.SendAnswer(url, question);
        AjaxUrl_GetAnswer = url => Links.GetAnswer(url, question);
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