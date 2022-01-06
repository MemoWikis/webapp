using NHibernate;

namespace TrueOrFalse.Updates
{
    class UpdateToVs229
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `user`
                    ADD COLUMN `BouncedMail` BIT,
                    ADD COLUMN `MailBounceReason` TEXT"
                ).ExecuteUpdate();
        }
    }
}
