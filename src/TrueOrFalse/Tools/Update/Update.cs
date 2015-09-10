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
                .Add(80, UpdateToVs080.Run)
                .Add(81, UpdateToVs081.Run)
                .Add(82, UpdateToVs082.Run)
                .Add(83, UpdateToVs083.Run)
                .Add(84, UpdateToVs084.Run)
                .Add(85, UpdateToVs085.Run)
                .Add(86, UpdateToVs086.Run)
                .Add(87, UpdateToVs087.Run)
                .Add(88, UpdateToVs088.Run)
                .Add(90, UpdateToVs090.Run)
                .Add(91, UpdateToVs091.Run)
                .Add(92, UpdateToVs092.Run)
                .Add(93, UpdateToVs093.Run)
                .Add(94, UpdateToVs094.Run)
                .Add(95, UpdateToVs095.Run)
                .Add(96, UpdateToVs096.Run)
                .Add(97, UpdateToVs097.Run)
                .Add(98, UpdateToVs098.Run)
                .Add(99, UpdateToVs099.Run)
                .Run();
        }
    }
}