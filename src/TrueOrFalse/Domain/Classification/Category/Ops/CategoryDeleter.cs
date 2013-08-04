using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace TrueOrFalse
{
    public class CategoryDeleter : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;

        public CategoryDeleter(ISession session){
            _session = session;
        }

        public void Run(Category category)
        {
            if (category == null)
                return;

            _session.CreateSQLQuery("DELETE FROM relatedcategoriestorelatedcategories where Related_id = " + category.Id).ExecuteUpdate();
            _session.CreateSQLQuery("DELETE FROM relatedcategoriestorelatedcategories where Category_id = " + category.Id).ExecuteUpdate();
            _session.CreateSQLQuery("DELETE FROM categoriestoquestions where Category_id = " + category.Id).ExecuteUpdate();
            _session.CreateSQLQuery("DELETE FROM category WHERE Id = " + category.Id).ExecuteUpdate();   
        }
    }
}
