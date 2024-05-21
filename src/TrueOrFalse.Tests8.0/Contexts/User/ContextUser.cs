public class ContextUser
{
    private readonly UserWritingRepo _userWritingRepo;

    public List<User> All = new();

    private ContextUser(UserWritingRepo userWritingRepo)
    {
        _userWritingRepo = userWritingRepo;
    }

    public User? UserGetByName(string name)
    {
        return All.SingleOrDefault(u => u.Name == name);
    }

    public static ContextUser New(UserWritingRepo userWritingRepo)
    {
        return new ContextUser(userWritingRepo);
    }

    public User GetUser(string userName)
    {
        return All.Single(u => u.Name.Equals(userName));
    }

    public ContextUser Add(string userName)
    {
        All.Add(new User { Name = userName });
        return this;
    }

    public ContextUser AddWithEmail(string mailAddress)
    {
        All.Add(new User { EmailAddress = mailAddress });
        return this;
    }

    public ContextUser Add()
    {
        All.Add(new User
        {
            Id = 0,
            Name = "Daniel"
        });
        return this;
    }

    public ContextUser Add(User user)
    {
        All.Add(user);
        return this;
    }

    public ContextUser Persist()
    {
        foreach (var usr in All)
            _userWritingRepo.Create(usr);

        return this;
    }
}