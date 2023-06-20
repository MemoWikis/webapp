using System;
using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

public class UsersModel : BaseModel
{
    public UIMessage Message;

    public string CanonicalUrl;
    public bool HasFiltersOrChangedOrder;
    public string PageTitle = "Nutzer";
    public bool ActiveTabAll = true;//$temp
    public bool ActiveTabFollowed;//$temp

    public string SearchTerm { get; set;  }

    public bool FilterByMe { get; set; }
    public bool FilterByAll { get; set; }

    public string OrderByLabel { get; set; }
    public UserOrderBy OrderBy;

    public PagerModel Pager { get; set; }

    public string Suggestion; 

    public IEnumerable<UserRowModel> Rows;

    public UserSearchResultModel SearchResultModel;
    public HeaderModel HeaderModel = new HeaderModel();

    public int TotalInResult;

    public UsersModel(){
    }

    public void Init(IList<User> users)
    {
        var counter = 0;

        Rows = users.Select(qs => 
            new UserRowModel(
                qs, counter++, R<FollowerIAm>().Init(users.Select(u => u.Id), UserId))
            );
        Pager = new PagerModel(_sessionUiData.SearchSpecUser);

        SearchResultModel = new UserSearchResultModel(this);

      
        SearchTerm = _sessionUiData.SearchSpecUser.SearchTerm;

        TotalInResult = _sessionUiData.SearchSpecUser.TotalItems;

        HeaderModel.TotalUsers = R<GetTotalUsers>().Run();

        OrderByLabel = _sessionUiData.SearchSpecUser.OrderBy.ToText();
        OrderBy = _sessionUiData.SearchSpecUser.OrderBy;


        if (!String.IsNullOrEmpty(SearchTerm) ||
            !(_sessionUiData.SearchSpecUser.OrderBy.Reputation.IsCurrent() || String.IsNullOrEmpty(OrderByLabel)))
            HasFiltersOrChangedOrder = true;
        CanonicalUrl = Links.Users();
        if (Pager.CurrentPage > 1)
        {
            CanonicalUrl += "?page=" + Pager.CurrentPage.ToString();
            PageTitle += " (Seite " + Pager.CurrentPage.ToString() + ")";
        }
        if (IsLoggedIn)
        {
            HeaderModel.TotalFollowingMe = R<TotalFollowers>().Run(UserId);
            HeaderModel.TotalIFollow = R<TotalIFollow>().Run(UserId);
        }
    }
}