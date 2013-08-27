using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using TrueOrFalse.Search;

namespace TrueOrFalse
{
    public class CategoryDeleter : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;
        private readonly RemoveCategoryFromIndex _removeCategoryFromIndex;

        public CategoryDeleter(
            ISession session, 
            RemoveCategoryFromIndex removeCategoryFromIndex)
        {
            _session = session;
            _removeCategoryFromIndex = removeCategoryFromIndex;
        }

        public void Run(Category category)
        {
            if (category == null)
                return;

            _removeCategoryFromIndex.Run(category);

            _session.CreateSQLQuery("DELETE FROM relatedcategoriestorelatedcategories where Related_id = " + category.Id).ExecuteUpdate();
            _session.CreateSQLQuery("DELETE FROM relatedcategoriestorelatedcategories where Category_id = " + category.Id).ExecuteUpdate();
            _session.CreateSQLQuery("DELETE FROM categoriestoquestions where Category_id = " + category.Id).ExecuteUpdate();
            _session.CreateSQLQuery("DELETE FROM category WHERE Id = " + category.Id).ExecuteUpdate();   
        }
    }
}