using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs172
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `answer`
                    DROP INDEX `PK_AnswerId`;"
            ).ExecuteUpdate();

            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `category`
                      DROP INDEX `CategoryId`;"
                ).ExecuteUpdate();

            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `persistentlogin`
                    DROP INDEX `PK__Persiste__3214EC074535E272`;"
                ).ExecuteUpdate();

            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `question`
                    DROP INDEX `QuestionId`;"
                ).ExecuteUpdate();

            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `questionvaluation`
                    DROP INDEX `PK__Question__3214EC0739C42FC6`;"
                ).ExecuteUpdate();

            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `questionview`
                    ADD PRIMARY KEY(`Id`);"
                ).ExecuteUpdate();

            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `questionview`
                    DROP INDEX `Id`;"
                ).ExecuteUpdate();

            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `setting`
                    DROP INDEX `PK__Setting__3214EC07612815AA`;"
                ).ExecuteUpdate();

            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `user`
                    DROP INDEX `UserId`;"
                ).ExecuteUpdate();
        }
    }
}