using System.Collections.Generic;
using TrueOrFalse.Search;

public class UsersControllerSearch
{
    public IList<User> Run()
    {
        var solrResult = Sl.R<SearchUsers>().Run(Sl.R<SessionUiData>().SearchSpecUser);
        return Sl.R<UserRepo>().GetByIds(solrResult.UserIds.ToArray());
    }
}