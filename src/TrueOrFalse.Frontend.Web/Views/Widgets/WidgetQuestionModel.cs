using static System.String;

public class WidgetQuestionModel : BaseModel
{
    public AnswerQuestionModel AnswerQuestionModel;
    public string QuestionText => AnswerQuestionModel.QuestionText;
    public Question Question => AnswerQuestionModel.Question;

    public bool IncludeCustomCss => !IsNullOrEmpty(CustomCss);
    public string CustomCss;

    public WidgetQuestionModel(AnswerQuestionModel answerQuestionModel, string host)
    {
        ShowUserReportWidget = false;
        AnswerQuestionModel = answerQuestionModel;

        if(host == "besser.memucho.de")
            CustomCss = "/Views/Widgets/CustomCss/besser.memucho.de.css";
    }
}