public class WidgetQuestionModel : BaseModel
{
    public AnswerQuestionModel AnswerQuestionModel;
    public Question Question => AnswerQuestionModel.Question;

    public WidgetQuestionModel(AnswerQuestionModel answerQuestionModel)
    {
        ShowUserReportWidget = false;
        AnswerQuestionModel = answerQuestionModel;
    }
}