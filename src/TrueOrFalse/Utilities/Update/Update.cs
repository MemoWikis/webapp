using TrueOrFalse;

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
                .Add(12, UpdateToVs012.Run)
                .Add(13, UpdateToVs013.Run)
                .Add(14, UpdateToVs014.Run)
                .Add(15, UpdateToVs015.Run)
                .Add(16, UpdateToVs016.Run)
                .Add(17, UpdateToVs017.Run)
                .Add(18, UpdateToVs018.Run)
                .Add(19, UpdateToVs019.Run)
                .Add(20, UpdateToVs020.Run)
                .Add(21, UpdateToVs021.Run)
                .Add(22, UpdateToVs022.Run)
                .Add(23, UpdateToVs023.Run)
                .Add(24, UpdateToVs024.Run)
                .Add(25, UpdateToVs025.Run)
                .Add(26, UpdateToVs026.Run)
                .Add(27, UpdateToVs027.Run)
                .Add(28, UpdateToVs028.Run)
                .Add(29, UpdateToVs029.Run)
                .Add(30, UpdateToVs030.Run)
                .Add(31, UpdateToVs031.Run)
                .Add(32, UpdateToVs032.Run)
                .Add(33, UpdateToVs033.Run)
                .Add(34, UpdateToVs034.Run)
                .Add(35, UpdateToVs035.Run)
                .Add(36, SolrCoreReload.ReloadUser)
                .Add(37, SolrCoreReload.ReloadSet)
                .Add(38, UpdateToVs038.Run)
                .Run();
        }
    }
}