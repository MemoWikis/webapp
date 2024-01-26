using NHibernate;

namespace TrueOrFalse.Updates;

public class Update : IRegisterAsInstancePerLifetime
{
    private readonly UpdateStepExecuter _updateStepExecuter;
    private readonly ISession _nhibernateSession;
    private CategoryRepository _categoryRepository;
    private UserReadingRepo _userReadingRepo;
    private UserWritingRepo _userWritingRepo;

    public Update(UpdateStepExecuter updateStepExecuter, ISession nhibernateSession, CategoryRepository categoryRepository)
    {
        _updateStepExecuter = updateStepExecuter;
        _nhibernateSession = nhibernateSession;
        _categoryRepository = categoryRepository;
    }

    public void Run()
    {
        _updateStepExecuter
            .Add(239, () => UpdateToVs239.Run(_nhibernateSession))
            .Add(240, () => UpdateToVs240.Run(_nhibernateSession))
            .Add(241, () => UpdateToVs241.Run(_nhibernateSession))
            .Add(242, () => UpdateToVs242.Run(_nhibernateSession))
            .Add(243, () => UpdateToVs243.Run(_nhibernateSession))
            .Add(244, () => UpdateToVs244.Run(_nhibernateSession))
            .Add(245, () => UpdateToVs245.Run(_nhibernateSession))
            .Add(246, () => UpdateToVs246.Run(_nhibernateSession))
            .Add(247, () => UpdateToVs247.Run(_nhibernateSession))
            .Add(248, () => UpdateToVs248.Run(_nhibernateSession))
            .Add(249, () => UpdateToVs249.Run(_nhibernateSession))
            .Add(250, () => UpdateToVs250.Run(_nhibernateSession))
            .Add(251, () => UpdateToVs251.Run(_nhibernateSession))
            .Add(252, () => UpdateToVs252.Run(_nhibernateSession))
            .Add(253, () => UpdateToVs253.Run(_nhibernateSession))
            .Add(254, () => UpdateToVs254.Run(_nhibernateSession))                               
            .Add(255, () => UpdateToVs255.Run(_nhibernateSession))
            .Add(256, () => UpdateToVs256.Run(_nhibernateSession))
            .Add(257, () => UpdateToVs257.Run(_nhibernateSession))
            .Add(258, () => UpdateToVs258.Run(_nhibernateSession))
            .Add(259, () => UpdateToVs259.Run(_nhibernateSession))
            .Add(260, () => UpdateToVs260.Run(_nhibernateSession))
            .Add(261, () => UpdateToVs261.Run(_nhibernateSession))
            .Add(262, () => UpdateToVs262.Run(_nhibernateSession))
            .Add(263, () => UpdateToVs263.Run(_nhibernateSession))
            .Add(264, () => UpdateToVs264.Run(_nhibernateSession))
            .Add(265, () => UpdateToVs265.Run(_nhibernateSession))
            .Add(266, () => UpdateToVs266.Run(_categoryRepository, _userWritingRepo, _userReadingRepo))
            .Add(267, () => UpdateToVs267.Run(_nhibernateSession))
            .Run();
    }
}