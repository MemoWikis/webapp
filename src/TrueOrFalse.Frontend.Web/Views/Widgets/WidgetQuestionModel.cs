public class WidgetQuestionModel : BaseModel
{
    public AnswerQuestionModel AnswerQuestionModel;

    public WidgetQuestionModel(AnswerQuestionModel answerQuestionModel)
    {
        ShowUserReportWidget = false;
        AnswerQuestionModel = answerQuestionModel;
    }
}