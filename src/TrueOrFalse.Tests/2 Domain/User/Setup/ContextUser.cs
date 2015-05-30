using System.Collections.Generic;
public class ContextUser : IRegisterAsInstancePerLifetime
{
    private readonly UserRepository _userRepository;

    public List<User> All = new List<User>();

    public ContextUser(UserRepository userRepository)
    {
        _userRepository = userRepository;
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
            _userRepository.Create(usr);

        return this;
    }
}
