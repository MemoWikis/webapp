using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Seedworks.Lib.Persistance;

namespace TrueOrFalse.Core
{
    public class CategoryRepository : RepositoryDb<Category>
    {
        public CategoryRepository(ISession session)
            : base(session)
        {
        }

        public override void Create(Category category)
        {
            InitializeNewSubCategories(category);
            base.Create(category);
            Flush();
        }

        public override void Update(Category category)
        {
            InitializeNewSubCategories(category);
            base.Update(category);
            Flush();
        }

        private static void InitializeNewSubCategories(Category category)
        {
            foreach (var subCategory in category.SubCategories.Where(subCategory => subCategory.Id == 0))
            {
                subCategory.DateCreated = DateTime.Now;
                subCategory.DateModified = DateTime.Now;

                foreach (var subCategoryItem in subCategory.Items.Where(subCategoryItem => subCategoryItem.Id == 0))
                {
                    subCategoryItem.DateCreated = DateTime.Now;
                    subCategoryItem.DateModified = DateTime.Now;
                }
            }
        }
    }
}
