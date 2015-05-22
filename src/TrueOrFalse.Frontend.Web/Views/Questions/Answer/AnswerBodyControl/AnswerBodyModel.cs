using System;
using System.Web.Mvc;

public class AnswerBodyModel
{
    public bool IsInWishknowledge;

    public string QuestionText;
    public string QuestionTextMarkdown;

    public bool HasSound;
    public string SoundUrl;
    
    public string SolutionMetaDataJson;
    public SolutionMetadata SolutionMetadata;
    public string SolutionType;
    public QuestionSolution SolutionModel;

    public Func<UrlHelper, string> NextUrl;

    public AnswerBodyModel(AnswerQuestionModel answerQuestionModel)
    {
        IsInWishknowledge = answerQuestionModel.IsInWishknowledge;

        QuestionText = answerQuestionModel.QuestionText;
        QuestionTextMarkdown = answerQuestionModel.QuestionTextMarkdown;

        HasSound = answerQuestionModel.HasSound;
        SoundUrl = answerQuestionModel.SoundUrl;

        SolutionMetaDataJson = answerQuestionModel.SolutionMetaDataJson;
        SolutionMetadata = answerQuestionModel.SolutionMetadata;
        SolutionType = answerQuestionModel.SolutionType;
        SolutionModel = answerQuestionModel.SolutionModel;

        NextUrl = answerQuestionModel.NextUrl;
    }
}
