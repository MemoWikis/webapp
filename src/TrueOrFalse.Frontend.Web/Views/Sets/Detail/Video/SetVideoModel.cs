using System.Linq;
using Seedworks.Lib.Persistence;

public class SetVideoModel : BaseModel
{
    public int QuestionCount;
    public AnswerBodyModel AnswerBodyModel;
    public PagerModel Pager;

    public string VideoKey;

    public SetVideoModel(Set set)
    {
        var answerQuestionModel = new AnswerQuestionModel(set.Questions().First());

        VideoKey = set.VideoKey;
        AnswerBodyModel = new AnswerBodyModel(answerQuestionModel);
        QuestionCount = set.Questions().Count;

        var pager = new Pager();
        pager.TotalItems = set.Questions().Count;
        pager.PageSize = 1;
        pager.CurrentPage = 1;

        Pager = new PagerModel(pager);
    }
}