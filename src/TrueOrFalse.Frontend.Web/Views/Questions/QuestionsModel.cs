using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Frontend.Web.Models;
using TrueOrFalse.Core;

public class QuestionsModel : ModelBase
{
    public QuestionsModel()
    {
        QuestionRows = Enumerable.Empty<QuestionRowModel>();
    }

    public QuestionsModel(IEnumerable<Question> questions)
    {
        QuestionRows = from question in questions select new QuestionRowModel(question);
    }

    public IEnumerable<QuestionRowModel> QuestionRows { get; set; }

    public PagerModel Pager { get; set; }

    public bool? FilterByMe { get; set; }
    public bool? FilterByAll { get; set; }
}