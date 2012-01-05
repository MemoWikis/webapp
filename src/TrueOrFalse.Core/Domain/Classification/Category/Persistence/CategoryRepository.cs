using System;
using System.Linq;
using NHibernate;
using Seedworks.Lib.Persistence;

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

        public Category GetByName(string categoryName)
        {
            return _session.CreateQuery("from Category as c where c.Name = :categoryName")
                           .SetString("categoryName", categoryName)
                           .UniqueResult<Category>();
        }

        public bool Exists(string categoryName)
        {
            return GetByName(categoryName) != null;
        }
    }
}
