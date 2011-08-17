using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core
{
    public class SubCategoryRepository : RepositoryDb<SubCategory>
    {
        public SubCategoryRepository(ISession session) : base(session)
        {
        }

        public override void Update(SubCategory subCategory)
        {
            foreach (var subCategoryItem in subCategory.Items.Where(subCategoryItem => subCategoryItem.Id == 0))
            {
                subCategoryItem.DateCreated = DateTime.Now;
                subCategoryItem.DateModified = DateTime.Now;
            }

            base.Update(subCategory);
        }
    }
}
