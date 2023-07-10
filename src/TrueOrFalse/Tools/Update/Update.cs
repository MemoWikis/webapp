using NHibernate;

namespace TrueOrFalse.Updates;

public class Update : IRegisterAsInstancePerLifetime
{
    private readonly UpdateStepExecuter _updateStepExecuter;
    private readonly ISession _nhibernateSession;

    public Update(UpdateStepExecuter updateStepExecuter, ISession nhibernateSession)
    {
        _updateStepExecuter = updateStepExecuter;
        _nhibernateSession = nhibernateSession;
    }

    public void Run()
    {
        _updateStepExecuter
            .Add(()=> UpdateToVs239.Run(_nhibernateSession))
            .Add(() => UpdateToVs240.Run(_nhibernateSession))
            .Add(() => UpdateToVs241.Run(_nhibernateSession))
            .Add(() => UpdateToVs242.Run(_nhibernateSession))
            .Add(() => UpdateToVs243.Run(_nhibernateSession))
            .Add(() => UpdateToVs244.Run(_nhibernateSession))
            .Add(() => UpdateToVs245.Run(_nhibernateSession))
            .Add(()=>UpdateToVs246.Run(_nhibernateSession))
            .Add(()=>UpdateToVs247.Run(_nhibernateSession))
            .Add(() => UpdateToVs248.Run(_nhibernateSession))
            .Add(() => UpdateToVs249.Run(_nhibernateSession))
            .Add(() => UpdateToVs250.Run(_nhibernateSession))
            .Add(() => UpdateToVs251.Run(_nhibernateSession))
            .Add(() => UpdateToVs252.Run(_nhibernateSession))
            .Add(() => UpdateToVs253.Run(_nhibernateSession))
            .Add(() => UpdateToVs254.Run(_nhibernateSession))                               
            .Add(() => UpdateToVs255.Run(_nhibernateSession))
            .Add(() => UpdateToVs256.Run(_nhibernateSession))
            .Add(() => UpdateToVs257.Run(_nhibernateSession))
            .Add(() => UpdateToVs258.Run(_nhibernateSession))
            .Add(() => UpdateToVs259.Run(_nhibernateSession))
            .Add(() => UpdateToVs260.Run(_nhibernateSession))
            .Add(() => UpdateToVs261.Run(_nhibernateSession) )
            .Add(() => UpdateToVs262.Run(_nhibernateSession))
            .Add(() => UpdateToVs263.Run(_nhibernateSession))
            .Add(() => UpdateToVs264.Run(_nhibernateSession))
            .Add(() => UpdateToVs265.Run(_nhibernateSession))
            .Run();
    }
}