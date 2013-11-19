using System.Collections.Generic;
using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Search;
using TrueOrFalse.Web.Context;

public class UsersControllerSearch : IRegisterAsInstancePerLifetime
{
    private readonly UserRepository _usersRepo;
    private readonly SessionUiData _sessionUiData;
    private readonly SearchUsers _searchUsers;

    public UsersControllerSearch(
        UserRepository usersRepo, 
        SessionUiData sessionUiData,
        SearchUsers searchUsers)
    {
        _usersRepo = usersRepo;
        _sessionUiData = sessionUiData;
        _searchUsers = searchUsers;
    }

    public IList<User> Run()
    {
        if (string.IsNullOrEmpty(_sessionUiData.SearchSpecUser.SearchTerm))
            return SearchFromSqlServer();
        
        return SearchFromSOLR();
    }

    private IList<User> SearchFromSOLR()
    {
        var solrResult = _searchUsers.Run(
            _sessionUiData.SearchSpecUser.SearchTerm,
            _sessionUiData.SearchSpecUser);

        return _usersRepo.GetByIds(solrResult.UserIds.ToArray());
    }

    private IList<User> SearchFromSqlServer()
    {
        return _usersRepo.GetBy(_sessionUiData.SearchSpecUser);
    }
}