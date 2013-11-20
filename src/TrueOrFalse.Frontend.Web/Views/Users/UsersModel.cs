using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Models;
using TrueOrFalse.Web;
using TrueOrFalse.Web.Context;

public class UsersModel : BaseModel
{
    public Message Message;

    public int TotalSets { get; set; }
    public int TotalMine { get; set; }

    public string SearchTerm { get; set;  }

    public bool FilterByMe { get; set; }
    public bool FilterByAll { get; set; }

    public PagerModel Pager { get; set; }

    public IEnumerable<UserRowModel> Rows;

    public UsersModel(){
    }

    public void Init(IEnumerable<User> users)
    {
        var counter = 0;
        Rows = users.Select(qs => new UserRowModel(qs, counter++, _sessionUser));

        SearchTerm = _sessionUiData.SearchSpecUser.SearchTerm;

        TotalSets = Resolve<GetTotalSetCount>().Run();

        Pager = new PagerModel(_sessionUiData.SearchSpecUser);
    }
}