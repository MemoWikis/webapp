using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs023
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>()
                .CreateSQLQuery(@"ALTER TABLE `trueorfalse`.`user` ADD COLUMN `AllowsSupportiveLogin` BIT(1) NULL DEFAULT 0  AFTER `IsInstallationAdmin` ;").ExecuteUpdate();
        }
    }
}
