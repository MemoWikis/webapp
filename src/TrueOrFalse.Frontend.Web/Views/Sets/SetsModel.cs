using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Frontend.Web.Models;
using TrueOrFalse.Web;
using TrueOrFalse.Web.Context;

public class SetsModel : BaseModel
{
    public Message Message;

    public bool ActiveTabAll;
    public bool ActiveTabMine;
    public bool ActiveTabWish;

    public int TotalSets { get; set; }
    public int TotalMine { get; set; }
    public int TotalWish { get; set; }

    public string SearchTerm { get; set;  }

    public bool FilterByMe { get; set; }
    public bool FilterByAll { get; set; }

    public PagerModel Pager { get; set; }

    public IEnumerable<SetRowModel> Rows;

    public SetsModel(){}

    public SetsModel(IEnumerable<Set> questionSets)
    {
        var counter = 0;
        Rows = questionSets.Select(qs => new SetRowModel(qs, counter++, _sessionUser.User.Id));

        TotalSets = Resolve<GetTotalSetCount>().Run();
        TotalMine = Resolve<GetTotalSetCount>().Run(_sessionUser.User.Id);
        SearchTerm = _sessionUiData.SearchSpecSet.SearchTearm;

        Pager = new PagerModel(_sessionUiData.SearchSpecSet);
        Pager.Controller = Links.SetsController;
        Pager.Action = Links.SetsAction;
    }
}