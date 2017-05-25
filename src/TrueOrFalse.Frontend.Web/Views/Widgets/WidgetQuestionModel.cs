public class WidgetQuestionModel : WidgetBaseModel
{
    public AnswerQuestionModel AnswerQuestionModel;
    public string QuestionText => AnswerQuestionModel.QuestionText;
    public Question Question => AnswerQuestionModel.Question;

    public WidgetQuestionModel(AnswerQuestionModel answerQuestionModel, string host) : base(host)
    {
        ShowUserReportWidget = false;
        AnswerQuestionModel = answerQuestionModel;
    }
}