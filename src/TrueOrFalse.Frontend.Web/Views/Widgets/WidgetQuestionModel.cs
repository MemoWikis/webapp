public class WidgetQuestionModel : BaseModel
{
    public AnswerQuestionModel AnswerQuestionModel;

    public WidgetQuestionModel(AnswerQuestionModel answerQuestionModel)
    {
        AnswerQuestionModel = answerQuestionModel;
    }
}