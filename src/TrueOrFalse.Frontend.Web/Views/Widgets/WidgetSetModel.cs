public class WidgetSetModel : WidgetBaseModel
{
    public AnswerQuestionModel AnswerQuestionModel;

    public bool IsTestSession => AnswerQuestionModel.IsTestSession;
    public int TestSessionId => AnswerQuestionModel.TestSessionId;
    public int TestSessionCurrentStep => AnswerQuestionModel.TestSessionCurrentStep;
    public bool TestSessionIsLastStep => AnswerQuestionModel.TestSessionIsLastStep;

    public string QuestionText => AnswerQuestionModel.QuestionText;
    public Question Question => AnswerQuestionModel.Question;

    public WidgetSetModel(AnswerQuestionModel answerQuestionModel, string host) : base(host)
    {
        ShowUserReportWidget = false;
        AnswerQuestionModel = answerQuestionModel;
    }
}