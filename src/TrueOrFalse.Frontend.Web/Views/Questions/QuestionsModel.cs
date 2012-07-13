using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Frontend.Web.Models;
using TrueOrFalse.Core;

public class QuestionsModel : ModelBase
{
    public QuestionsModel()
    {
        QuestionRows = Enumerable.Empty<QuestionRowModel>();
        FilterByUsers = new Dictionary<int, string>();
    }

    public QuestionsModel(IEnumerable<Question> questions, 
                          IEnumerable<TotalPerUser> totalsForUser,
                          QuestionSearchSpec questionSearchSpec, 
                          int currentUserId)
    {
        int counter = 0; 
        QuestionRows = from question in questions
                       select new QuestionRowModel(
                                    question,
                                    totalsForUser.ByQuestionId(question.Id),
                                    ((questionSearchSpec.CurrentPage - 1) * questionSearchSpec.PageSize) + ++counter, 
                                    currentUserId
                                  );

        FilterByUsers = new Dictionary<int, string>();
        TotalQuestions = questionSearchSpec.TotalItems;
    }

    public IEnumerable<QuestionRowModel> QuestionRows { get; set; }

    public PagerModel Pager { get; set; }

    public bool? FilterByMe { get; set; }
    public bool? FilterByAll { get; set; }
    public int? AddFilterUser { get; set; }
    public int? DelFilterUser { get; set; }
    public Dictionary<int, string> FilterByUsers { get; set; }

    public int TotalQuestions;
}