using System.Collections.Generic;
using TrueOrFalse.Search;

public class UsersControllerSearch : IRegisterAsInstancePerLifetime
{
    private readonly UserRepo _usersRepo;
    private readonly SessionUiData _sessionUiData;
    private readonly SearchUsers _searchUsers;

    public UsersControllerSearch(
        UserRepo usersRepo, 
        SessionUiData sessionUiData,
        SearchUsers searchUsers)
    {
        _usersRepo = usersRepo;
        _sessionUiData = sessionUiData;
        _searchUsers = searchUsers;
    }

    public IList<User> Run()
    {
        var solrResult = _searchUsers.Run(_sessionUiData.SearchSpecUser);

        return _usersRepo.GetByIds(solrResult.UserIds.ToArray());
    }
}