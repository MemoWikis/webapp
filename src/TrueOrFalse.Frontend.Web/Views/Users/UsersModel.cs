﻿using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Web;

public class UsersModel : BaseModel
{
    public UIMessage Message;

    public bool ActiveTabAll = true;//$temp
    public bool ActiveTabFollowed;//$temp

    public int TotalUsers { get; set; }
    public int TotalMine { get; set; }

    public string SearchTerm { get; set;  }

    public bool FilterByMe { get; set; }
    public bool FilterByAll { get; set; }

    public string OrderByLabel { get; set; }
    public UserOrderBy OrderBy;

    public PagerModel Pager { get; set; }

    public string Suggestion; 

    public IEnumerable<UserRowModel> Rows;

    public UsersModel(){
    }

    public void Init(IList<User> users)
    {
        var counter = 0;

        Rows = users.Select(qs => 
            new UserRowModel(
                qs, counter++, _sessionUser, 
                R<FollowerIAm>().Init(users.Select(u => u.Id), UserId))
            );

        Suggestion = _sessionUiData.SearchSpecUser.GetSuggestion();
        SearchTerm = _sessionUiData.SearchSpecUser.SearchTerm;

        TotalUsers = Resolve<GetTotalUsers>().Run();

        Pager = new PagerModel(_sessionUiData.SearchSpecUser);

        OrderByLabel = _sessionUiData.SearchSpecUser.OrderBy.ToText();
        OrderBy = _sessionUiData.SearchSpecUser.OrderBy;
    }
}