using System.Collections.Generic;
using TrueOrFalse.Search;

public class UsersControllerSearch
{
    public IList<User> Run()
    {
        var solrResult = Sl.SolrSearchUsers.Run(Sl.SessionUiData.SearchSpecUser);
        return Sl.UserRepo.GetByIds(solrResult.UserIds.ToArray());
    }
}