namespace TrueOrFalse.Updates;

public class Update : IRegisterAsInstancePerLifetime
{
    private readonly UpdateStepExecuter _updateStepExecuter;

    public Update(UpdateStepExecuter updateStepExecuter)
    {
        _updateStepExecuter = updateStepExecuter;
    }

    public void Run()
    {
        _updateStepExecuter
            .Add(UpdateToVs228.Run)
            .Add(UpdateToVs229.Run)
            .Add(UpdateToVs230.Run)
            .Add(UpdateToVs231.Run)
            .Add(UpdateToVs232.Run)
            .Add(UpdateToVs234.Run)
            .Add(UpdateToVs235.Run)
            .Add(UpdateToVs236.Run)
            .Add(UpdateToVs237.Run)
            .Add(UpdateToVs238.Run)
            .Add(UpdateToVs239.Run)
            .Add(UpdateToVs240.Run)
            .Add(UpdateToVs241.Run)
            .Add(UpdateToVs242.Run)
            .Add(UpdateToVs243.Run)
            .Add(UpdateToVs244.Run)
            .Add(UpdateToVs245.Run)
            .Add(UpdateToVs246.Run)
            .Add(UpdateToVs247.Run)
            .Add(UpdateToVs248.Run)
            .Add(UpdateToVs249.Run)
            .Add(UpdateToVs250.Run)
            .Add(UpdateToVs251.Run)
            .Add(UpdateToVs252.Run)
            .Add(UpdateToVs253.Run)
            .Add(UpdateToVs254.Run)
            .Add(UpdateToVs255.Run)
            .Add(UpdateToVs256.Run)
            .Add(UpdateToVs257.Run)
            .Add(UpdateToVs258.Run)
            .Add(UpdateToVs259.Run)
            .Add(UpdateToVs260.Run)
            .Add(UpdateToVs261.Run)
            .Add(UpdateToVs262.Run)
            .Run();
    }
}