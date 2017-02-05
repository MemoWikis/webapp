using System.Collections.Generic;
using System.Linq;

public class SetVideoModel : BaseModel
{
    public AnswerBodyModel AnswerBodyModel;
    public string VideoKey;

    public int QuestionCount;
    public IList<Question> Questions;
    public int CurrentQuestion => AnswerBodyModel.QuestionId;

    public SetVideoModel(Set set)
    {
        var answerQuestionModel = new AnswerQuestionModel(set.Questions().First());

        VideoKey = set.VideoKey;
        AnswerBodyModel = new AnswerBodyModel(answerQuestionModel);

        QuestionCount = set.Questions().Count;
        Questions = set.Questions();
    }
}