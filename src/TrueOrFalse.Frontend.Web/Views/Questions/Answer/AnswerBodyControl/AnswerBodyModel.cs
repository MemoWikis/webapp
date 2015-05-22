using System;
using System.Web.Mvc;
using TrueOrFalse.Web;

public class AnswerBodyModel : BaseModel
{
    public bool IsInWishknowledge;

    public string QuestionText;
    public string QuestionTextMarkdown;

    public bool HasSound{ get { return !string.IsNullOrEmpty(SoundUrl); } }
    public string SoundUrl;
    
    public string SolutionMetaDataJson;
    public SolutionMetadata SolutionMetadata;
    public string SolutionType;
    public QuestionSolution SolutionModel;

    public Func<UrlHelper, string> NextUrl;

    public AnswerBodyModel(Question question)
    {
        var questionValuationForUser = NotNull.Run(Resolve<QuestionValuationRepository>().GetBy(question.Id, UserId));
        IsInWishknowledge = questionValuationForUser.IsSetRelevancePersonal();

        QuestionText = question.Text;
        QuestionTextMarkdown = MardownInit.Run().Transform(question.TextExtended);

        SoundUrl = new GetQuestionSoundUrl().Run(question);

        SolutionMetadata = new SolutionMetadata { Json = question.SolutionMetadataJson };
        SolutionMetaDataJson = question.SolutionMetadataJson;
        SolutionType = question.SolutionType.ToString();
        SolutionModel = new GetQuestionSolution().Run(question);
    }

    public AnswerBodyModel(AnswerQuestionModel answerQuestionModel)
    {
        IsInWishknowledge = answerQuestionModel.IsInWishknowledge;

        QuestionText = answerQuestionModel.QuestionText;
        QuestionTextMarkdown = answerQuestionModel.QuestionTextMarkdown;

        SoundUrl = answerQuestionModel.SoundUrl;

        SolutionMetaDataJson = answerQuestionModel.SolutionMetaDataJson;
        SolutionMetadata = answerQuestionModel.SolutionMetadata;
        SolutionType = answerQuestionModel.SolutionType;
        SolutionModel = answerQuestionModel.SolutionModel;

        NextUrl = answerQuestionModel.NextUrl;
    }
}
