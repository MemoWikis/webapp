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
                .Run();
        }

    }
}
