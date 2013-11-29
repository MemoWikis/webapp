using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs029
    {
        public static void Run()
        {
            SolrCoreReload.ReloadCategory();       
        }
    }
}
