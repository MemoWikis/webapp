using System.Collections.Generic;
using System.Linq;
using NHibernate;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;

namespace TrueOrFalse
{
    public class SetRepository : RepositoryDb<Set>
    {
        private readonly SearchIndexSet _searchIndexSet;

        public SetRepository(ISession session, SearchIndexSet searchIndexSet)
            : base(session)
        {
            _searchIndexSet = searchIndexSet;
        }

        public override void Update(Set set)
        {
            _searchIndexSet.Update(set);
            base.Update(set);

            var categoriesToUpdate =
                _session.CreateSQLQuery("SELECT Category_id FROM categories_to_sets WHERE Set_id =" + set.Id)
                .List<int>().ToList();

            categoriesToUpdate.AddRange(set.Categories.Select(x => x.Id).ToList());
            categoriesToUpdate = categoriesToUpdate.GroupBy(x => x).Select(x => x.First()).ToList();
            Sl.Resolve<UpdateSetCountForCategory>().Run(categoriesToUpdate);
        }

        public override void Create(Set set)
        {
            base.Create(set);
            Sl.Resolve<UpdateSetCountForCategory>().Run(set.Categories);
            _searchIndexSet.Update(set);
        }

        public override void Delete(int id)
        {
            var set = GetById(id);
            _searchIndexSet.Delete(set);
            base.Delete(id);
        }

        public IList<Set>GetForCategory(int categoryId)
        {
            return _session.QueryOver<Set>()
                .JoinQueryOver<Category>(q => q.Categories)
                .Where(c => c.Id == categoryId)
                .List<Set>();
        }

    }
}
