using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Models;
using TrueOrFalse.Web;
using TrueOrFalse.Web.Context;

public class SetsModel : BaseModel
{
    public Message Message;

    public int TotalQuestionSets { get; set; }
    public int TotalMine { get; set; }

    public string SearchTerm { get; set;  }

    public bool FilterByMe { get; set; }
    public bool FilterByAll { get; set; }

    public PagerModel Pager { get; set; }

    public IEnumerable<SetRowModel> Rows;

    public SetsModel(){
    }

    public SetsModel(IEnumerable<Set> questionSets, SessionUser sessionUser)
    {
        var counter = 0;
        Rows = questionSets.Select(qs => new SetRowModel(qs, counter++, sessionUser.User.Id));

        TotalQuestionSets = Resolve<GetTotalSetCount>().Run();

        Pager = new PagerModel(_sessionUiData.SearchSpecSet);
    }
}