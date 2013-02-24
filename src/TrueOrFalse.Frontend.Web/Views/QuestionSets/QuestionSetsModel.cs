using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Models;
using TrueOrFalse.Web;
using TrueOrFalse.Web.Context;


public class QuestionSetsModel : BaseModel
{
    public Message Message;

    public int TotalQuestionSets { get; set; }
    public int TotalMine { get; set; }

    public string SearchTerm { get; set;  }

    public bool FilterByMe { get; set; }
    public bool FilterByAll { get; set; }

    public IEnumerable<QuestionSetRowModel> Rows;

    public QuestionSetsModel(IEnumerable<QuestionSet> questionSets, SessionUser sessionUser)
    {
        var counter = 0;
        Rows = questionSets.Select(qs => new QuestionSetRowModel(qs, counter++, sessionUser.User.Id));

        TotalQuestionSets = Resolve<GetTotalQuestionSetCount>().Run();
    }
}
