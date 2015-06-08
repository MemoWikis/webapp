using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs059
    {
        public static void Run()
        {
            var session = Sl.Resolve<ISession>();

            var dailies = session
                .QueryOver<Category>()
                .Where(c => c.Type == CategoryType.Daily)
                .List<Category>();

            foreach (var daily in dailies)
            {
                var typeModel = (CategoryTypeDaily)daily.GetTypeModel();
                typeModel.Title = daily.Name;
                daily.TypeJson = typeModel.ToJson();

                session.Update(daily);
            }
        }
    }
}