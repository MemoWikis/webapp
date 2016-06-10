namespace TrueOrFalse.Updates
{
    public class Update : IRegisterAsInstancePerLifetime
    {
        private readonly UpdateStepExecuter _updateStepExecuter;

        public Update(UpdateStepExecuter updateStepExecuter){
            _updateStepExecuter = updateStepExecuter;
        }

        public void Run()
        {
            _updateStepExecuter
                .Add(UpdateToVs080.Run)
                .Add(UpdateToVs081.Run)
                .Add(UpdateToVs082.Run)
                .Add(UpdateToVs083.Run)
                .Add(UpdateToVs084.Run)
                .Add(UpdateToVs085.Run)
                .Add(UpdateToVs086.Run)
                .Add(UpdateToVs087.Run)
                .Add(UpdateToVs088.Run)
                .Add(UpdateToVs090.Run)
                .Add(UpdateToVs091.Run)
                .Add(UpdateToVs092.Run)
                .Add(UpdateToVs093.Run)
                .Add(UpdateToVs094.Run)
                .Add(UpdateToVs095.Run)
                .Add(UpdateToVs096.Run)
                .Add(UpdateToVs097.Run)
                .Add(UpdateToVs098.Run)
                .Add(UpdateToVs099.Run)
                .Add(UpdateToVs100.Run)
                .Add(UpdateToVs101.Run)
                .Add(UpdateToVs102.Run)
                .Add(UpdateToVs103.Run)
                .Add(UpdateToVs104.Run)
                .Add(UpdateToVs105.Run)
                .Add(UpdateToVs106.Run)
                .Add(UpdateToVs107.Run)
                .Add(UpdateToVs108.Run)
                .Add(UpdateToVs109.Run)
                .Add(UpdateToVs110.Run)
                .Add(UpdateToVs111.Run)
                .Add(UpdateToVs112.Run)
                .Add(UpdateToVs113.Run)
                .Add(UpdateToVs114.Run)
                .Add(UpdateToVs115.Run)
                .Add(UpdateToVs116.Run)
                .Add(UpdateToVs117.Run)
                .Add(UpdateToVs118.Run)
                .Add(UpdateToVs119.Run)
                .Add(UpdateToVs120.Run)
                .Add(UpdateToVs121.Run)
                .Add(UpdateToVs122.Run)
                .Add(UpdateToVs123.Run)
                .Add(UpdateToVs124.Run)
                .Add(UpdateToVs125.Run)
                .Add(UpdateToVs126.Run)
                .Add(UpdateToVs127.Run)
                .Add(UpdateToVs128.Run)
                .Add(UpdateToVs129.Run)
                .Add(UpdateToVs134.Run)
                .Run();
        }
    }
}