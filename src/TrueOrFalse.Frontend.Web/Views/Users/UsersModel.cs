using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Web;

public class UsersModel : BaseModel
{
    public UIMessage Message;

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

        Suggestion = _sessionUiData.SearchSpecUser.GetSuggestion();
        SearchTerm = _sessionUiData.SearchSpecUser.SearchTerm;

        HeaderModel.TotalUsers = R<GetTotalUsers>().Run();

        OrderByLabel = _sessionUiData.SearchSpecUser.OrderBy.ToText();
        OrderBy = _sessionUiData.SearchSpecUser.OrderBy;

        if (!IsLoggedIn)
            return;

        HeaderModel.TotalFollowingMe = R<TotalFollowers>().Run(UserId);
        HeaderModel.TotalIFollow = R<TotalIFollow>().Run(UserId);
    }
}