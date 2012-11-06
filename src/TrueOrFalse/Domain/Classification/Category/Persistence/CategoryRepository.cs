using System;
using System.Linq;
using NHibernate;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    public class CategoryRepository : RepositoryDb<Category>
    {
        public CategoryRepository(ISession session)
            : base(session)
        {
        }

        public override void Create(Category category)
        {
            foreach (var related in category.RelatedCategories.Where(x => x.DateCreated == default(DateTime)))
                related.DateModified = related.DateCreated = DateTime.Now;
            base.Create(category);
            Flush();
        }

        public override void Update(Category category)
        {
            base.Update(category);
            Flush();
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
