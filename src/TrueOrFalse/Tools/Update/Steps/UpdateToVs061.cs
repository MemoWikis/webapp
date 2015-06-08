using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs061
    {
        public static void Run()
        {
            var session = Sl.Resolve<ISession>();

            var magazines = session
                .QueryOver<Category>()
                .Where(c => c.Type == CategoryType.Magazine)
                .List<Category>();

            foreach (var magazine in magazines)
            {
                var typeModel = (CategoryTypeMagazine)magazine.GetTypeModel();
                typeModel.Title = magazine.Name;
                magazine.TypeJson = typeModel.ToJson();

                session.Update(magazine);
            }
        }
    }
}