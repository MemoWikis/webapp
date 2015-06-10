using System.Collections.Generic;
public class ContextUser : IRegisterAsInstancePerLifetime
{
    private readonly UserRepo _userRepo;

    public List<User> All = new List<User>();

    public ContextUser(UserRepo userRepo)
    {
        _userRepo = userRepo;
    }

    public static ContextUser New()
    {
        return BaseTest.Resolve<ContextUser>();
    }

    public ContextUser Add(string userName)
    {
        All.Add(new User {Name = userName});
        return this;
    }

    public ContextUser Persist()
    {
        foreach (var usr in All)
            _userRepo.Create(usr);

        return this;
    }
}
