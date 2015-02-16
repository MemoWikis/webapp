using System.Collections.Generic;
using System.Linq;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

public class SetsModel : BaseModel
{
    public UIMessage Message;

    public bool ActiveTabAll;
    public bool ActiveTabMine;
    public bool ActiveTabWish;

    public int TotalSetsInSystem;
    public int TotalSetsInResult;
    public int TotalMine;
    public int TotalWish;

    public string SearchTerm { get; set; }
    public string SearchUrl;

    public string OrderByLabel;
    public SetOrderBy OrderBy;

    public bool FilterByMe;
    public bool FilterByAll;

    public PagerModel Pager;

    public string Suggestion; 

    public IEnumerable<SetRowModel> Rows;
    public SetsSearchResultModel SearchResultModel;

    public bool AccessNotAllowed;

    
    public SetsModel(){}

    public SetsModel(
        IList<Set> questionSets, 
        SetSearchSpec searchSpec,
        SearchTab searchTab
    )
    {
        ActiveTabAll = searchTab == SearchTab.All;
        ActiveTabMine = searchTab == SearchTab.Mine;
        ActiveTabWish = searchTab == SearchTab.Wish;

        AccessNotAllowed = !_sessionUser.IsLoggedIn && !ActiveTabAll;

        OrderBy = searchSpec.OrderBy;
        OrderByLabel = searchSpec.OrderBy.ToText();

        var valuations = R<SetValuationRepository>().GetBy(questionSets.GetIds(), _sessionUser.UserId);

        var counter = 0;
        Rows = questionSets.Select(set => 
            new SetRowModel(
                set,
                NotNull.Run(valuations.BySetId(set.Id)),
                counter++, 
                _sessionUser.UserId
            ));

        TotalSetsInSystem = Resolve<GetTotalSetCount>().Run();
        TotalMine = Resolve<GetTotalSetCount>().Run(_sessionUser.UserId);
        TotalWish = Resolve<GetWishSetCount>().Run(_sessionUser.UserId);

        TotalSetsInResult = searchSpec.TotalItems;

        SearchTerm = searchSpec.SearchTerm;
        Suggestion = searchSpec.GetSuggestion();

        Pager = new PagerModel(searchSpec) {Controller = Links.SetsController};

        if (ActiveTabAll){
            Pager.Action = Links.SetsAction;
            SearchUrl = "/FrageSaetze/Suche";
        }else if (ActiveTabWish){
            Pager.Action = Links.SetsWishAction;
            SearchUrl = "/FrageSaetze/Wunschwissen/Suche";
        }else if (ActiveTabMine){
            Pager.Action = Links.SetsMineAction;
            SearchUrl = "/FrageSaetze/Meine/Suche";
        }

        SearchResultModel = new SetsSearchResultModel(this);
    }
}