using NHibernate;

public class Update : IRegisterAsInstancePerLifetime
{
    private readonly UpdateStepExecuter _updateStepExecuter;
    private readonly ISession _nhibernateSession;
    private PageRepository _pageRepository;
    private UserReadingRepo _userReadingRepo;
    private UserWritingRepo _userWritingRepo;

    public Update(
        UpdateStepExecuter updateStepExecuter,
        ISession nhibernateSession,
        PageRepository pageRepository,
        UserWritingRepo userWritingRepo,
        UserReadingRepo userReadingRepo)
    {
        _updateStepExecuter = updateStepExecuter;
        _nhibernateSession = nhibernateSession;
        _pageRepository = pageRepository;
        _userWritingRepo = userWritingRepo;
        _userReadingRepo = userReadingRepo;
    }

    public void Run()
    {
        _updateStepExecuter
            .Add(279, () => UpdateToVs279.Run(_nhibernateSession))
            .Add(280, () => UpdateToVs280.Run(_nhibernateSession))
            .Add(281, () => UpdateToVs281.Run(_nhibernateSession))
            .Add(282, () => UpdateToVs282.Run(_nhibernateSession))
            .Add(283, () => UpdateToVs283.Run(_nhibernateSession))
            .Add(284, () => UpdateToVs284.Run(_nhibernateSession))
            .Run();
    }
}