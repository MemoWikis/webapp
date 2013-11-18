using System;
using System.Linq;
using NHibernate;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;

namespace TrueOrFalse
{
    public class CategoryRepository : RepositoryDb<Category>
    {
        private readonly SearchIndexCategory _searchIndexCategory;

        public CategoryRepository(ISession session, SearchIndexCategory searchIndexCategory)
            : base(session){
            _searchIndexCategory = searchIndexCategory;
        }

        public override void Create(Category category)
        {
            foreach (var related in category.RelatedCategories.Where(x => x.DateCreated == default(DateTime)))
                related.DateModified = related.DateCreated = DateTime.Now;
            base.Create(category);
            Flush();
            _searchIndexCategory.Update(category);
        }

        public override void Update(Category category)
        {
            _searchIndexCategory.Update(category);
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
