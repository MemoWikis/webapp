using System.Collections.Generic;

public class UserSearchResultModel : BaseModel
{
    public IEnumerable<UserRowModel> Rows { get; set; }
    public PagerModel Pager { get; set; }

    public UserSearchResultModel(UsersModel usersModel)
    {
        Rows = usersModel.Rows;
        Pager = usersModel.Pager;
    }
}
