using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs189
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"delete from categorychange"
            ).ExecuteUpdate();
        }
    }
}

