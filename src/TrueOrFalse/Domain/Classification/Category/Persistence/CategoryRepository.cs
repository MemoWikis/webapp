using System;
using System.Collections.Generic;
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
            foreach (var related in category.ParentCategories.Where(x => x.DateCreated == default(DateTime)))
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

        public IList<Category> GetByIds(List<int> questionIds)
        {
            return GetByIds(questionIds.ToArray());
        }

        public override IList<Category> GetByIds(params int[] questionIds)
        {
            var questions = base.GetByIds(questionIds);
            return questionIds.Select(t => questions.First(q => q.Id == t)).ToList();
        }

        public bool Exists(string categoryName)
        {
            return GetByName(categoryName) != null;
        }

        public IList<Category> GetChildren(int categoryId)
        {
            var categoryIds = _session.CreateSQLQuery(@"SELECT Category_id
                FROM relatedcategoriestorelatedcategories
                WHERE  Related_id = " + categoryId).List<int>();

            return GetByIds(categoryIds.ToArray());
        }
    }
}
