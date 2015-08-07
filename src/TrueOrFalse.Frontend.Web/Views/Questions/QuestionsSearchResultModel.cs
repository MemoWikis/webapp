using System.Collections.Generic;

public class QuestionsSearchResultModel : BaseModel
{
    public bool AccessNotAllowed;
    public IEnumerable<QuestionRowModel> QuestionRows { get; set; }
    public PagerModel Pager { get; set; }

    public QuestionsSearchResultModel(QuestionsModel questionsModel)
    {
        AccessNotAllowed = questionsModel.NotAllowed;
        QuestionRows = questionsModel.QuestionRows;
        Pager = questionsModel.Pager;
    }
}
