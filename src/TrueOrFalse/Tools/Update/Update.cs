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
                .Add(100, UpdateToVs100.Run)
                .Add(101, UpdateToVs101.Run)
                .Add(102, UpdateToVs102.Run)
                .Add(103, UpdateToVs103.Run)
                .Add(104, UpdateToVs104.Run)
                .Add(105, UpdateToVs105.Run)
                .Add(106, UpdateToVs106.Run)
                .Add(107, UpdateToVs107.Run)
                .Add(108, UpdateToVs108.Run)
                .Add(109, UpdateToVs109.Run)
                .Add(110, UpdateToVs110.Run)
                .Add(111, UpdateToVs111.Run)
                .Add(112, UpdateToVs112.Run)
                .Add(113, UpdateToVs113.Run)
                .Add(114, UpdateToVs114.Run)
                .Add(115, UpdateToVs115.Run)
                .Add(116, UpdateToVs116.Run)
                .Run();
        }
    }
}