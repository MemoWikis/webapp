using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Models;
using TrueOrFalse.Core;

public class QuestionsModel : ModelBase
{
    public QuestionsModel(IEnumerable<Question> questions)
    {
        ShowLeftMenu_Nav();

        QuestionRows = from question in questions select new QuestionRowModel(question);
        MainFullWidth = true;
    }

    public IEnumerable<QuestionRowModel> QuestionRows { get; set; }

}