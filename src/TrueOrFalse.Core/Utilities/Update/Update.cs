using TrueOrFalse.Core;

namespace TrueOrFalse.Updates
{
    public class Update : IRegisterAsInstancePerLifetime
    {
        private readonly UpdateStepExcecuter _updateStepExcecuter;

        public Update(UpdateStepExcecuter updateStepExcecuter)
        {
            _updateStepExcecuter = updateStepExcecuter;
        }

        public void Run()
        {
            _updateStepExcecuter
                .Add(1, UpdateToVs1.Run)
                .Add(2, UpdateToVs2.Run)
                .Add(3, UpdateToVs3.Run)
                .Run();
        }

    }
}
