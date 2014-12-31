using System.Drawing;
using System.IO;
using System.Linq;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs068
    {
        public static void Run()
        {
            var session = ServiceLocator.R<ISession>();
            session
                .CreateSQLQuery("DELETE FROM questionview WHERE UserId = -1")
                .ExecuteUpdate();
        }
    }
}
