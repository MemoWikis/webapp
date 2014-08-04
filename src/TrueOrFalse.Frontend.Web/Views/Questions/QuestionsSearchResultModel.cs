using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class QuestionsSearchResultModel : BaseModel
{
    public bool NotAllowed;
    public IEnumerable<QuestionRowModel> QuestionRows { get; set; }
    public PagerModel Pager { get; set; }

    public QuestionsSearchResultModel(QuestionsModel questionsModel)
    {
        NotAllowed = questionsModel.NotAllowed;
        QuestionRows = questionsModel.QuestionRows;
        Pager = questionsModel.Pager;
    }
}
