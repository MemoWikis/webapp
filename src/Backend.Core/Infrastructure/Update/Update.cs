using NHibernate;

public class Update(
    UpdateStepExecuter _updateStepExecutor, 
    ISession _nhibernateSession)
    : IRegisterAsInstancePerLifetime
{
    public void Run()
    {
        _updateStepExecutor
            .Add(279, () => UpdateToVs279.Run(_nhibernateSession))
            .Add(280, () => UpdateToVs280.Run(_nhibernateSession))
            .Add(281, () => UpdateToVs281.Run(_nhibernateSession))
            .Add(282, () => UpdateToVs282.Run(_nhibernateSession))
            .Add(283, () => UpdateToVs283.Run(_nhibernateSession))
            .Add(284, () => UpdateToVs284.Run(_nhibernateSession))
            .Add(285, () => UpdateToVs285.Run(_nhibernateSession))
            .Add(286, () => UpdateToVs286.Run(_nhibernateSession))
            .Add(287, () => UpdateToVs287.Run(_nhibernateSession))
            .Add(288, () => UpdateToVs288.Run(_nhibernateSession))
            .Add(289, () => UpdateToVs289.Run(_nhibernateSession))
            .Run();
    }
}