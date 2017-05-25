using System;
using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;
using static System.String;

public class SetsModel : BaseModel
{
    public UIMessage Message;

    public string CanonicalUrl;
    public bool HasFiltersOrChangedOrder;
    public string PageTitle = "Lernsets";

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
        SearchTabType searchTab
    )
    {
        ActiveTabAll = searchTab == SearchTabType.All;
        ActiveTabMine = searchTab == SearchTabType.Mine;
        ActiveTabWish = searchTab == SearchTabType.Wish;

        AccessNotAllowed = !_sessionUser.IsLoggedIn && !ActiveTabAll;

        OrderBy = searchSpec.OrderBy;
        OrderByLabel = searchSpec.OrderBy.ToText();

        var valuations = R<SetValuationRepo>().GetBy(questionSets.GetIds(), _sessionUser.UserId);

        Rows = questionSets.Select(set => 
            new SetRowModel(
                set,
                NotNull.Run(valuations.BySetId(set.Id)),
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
            SearchUrl = "/Fragesaetze/Suche";
        }else if (ActiveTabWish){
            Pager.Action = Links.SetsWishAction;
            SearchUrl = "/Fragesaetze/Wunschwissen/Suche";
        }else if (ActiveTabMine){
            Pager.Action = Links.SetsMineAction;
            SearchUrl = "/Fragesaetze/Meine/Suche";
        }

        SearchResultModel = new SetsSearchResultModel(this);

        /* Generate Canonical URL: Ignores search specifications and filters */
        if (!IsNullOrEmpty(SearchTerm) ||
            !(searchSpec.OrderBy.BestMatch.IsCurrent() || IsNullOrEmpty(OrderByLabel)))
            HasFiltersOrChangedOrder = true;

        if (ActiveTabAll)
            CanonicalUrl = Links.SetsAll();
        else if (ActiveTabWish)
            CanonicalUrl = Links.SetsWish();
        else if (ActiveTabMine)
            CanonicalUrl = Links.SetsMine();

        if (Pager.CurrentPage > 1)
        {
            CanonicalUrl += "?page=" + Pager.CurrentPage;
            PageTitle += " (Seite " + Pager.CurrentPage + ")";
        }
    }
}