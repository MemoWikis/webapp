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

    public int TotalSets { get; private set; }
    public int TotalMine { get; private set; }
    public int TotalWish { get; private set; }

    public string SearchTerm { get; set;  }
    public string SearchUrl { get; set; }

    public bool FilterByMe { get; set; }
    public bool FilterByAll { get; set; }

    public PagerModel Pager { get; set; }

    public IEnumerable<SetRowModel> Rows;
    
    public SetsModel(){}

    public SetsModel(
        IEnumerable<Set> questionSets, 
        SetSearchSpec searchSpec,
        bool isTabAllActive = false, 
        bool isTabWishActice = false,
        bool isTabMineActive = false
    )
    {
        ActiveTabAll = isTabAllActive;
        ActiveTabMine = isTabMineActive;
        ActiveTabWish = isTabWishActice;

        var counter = 0;
        Rows = questionSets.Select(qs => new SetRowModel(qs, counter++, _sessionUser.User.Id));

        TotalSets = Resolve<GetTotalSetCount>().Run();
        TotalMine = Resolve<GetTotalSetCount>().Run(_sessionUser.User.Id);
        
        SearchTerm = searchSpec.SearchTearm;
        Pager = new PagerModel(searchSpec) {Controller = Links.SetsController};

        if (ActiveTabAll){
            Pager.Action = Links.SetsAction;
            SearchUrl = "/FrageSaetze/Suche/";
        }else if (ActiveTabWish){
            Pager.Action = Links.SetsWishAction;
            SearchUrl = "/FrageSaetze/Wunschwissen/Suche/";
        }else if (ActiveTabMine){
            Pager.Action = Links.SetsMineAction;
            SearchUrl = "/FrageSaetze/Meine/Suche/";
        }
    }
}