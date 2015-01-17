using System.Collections.Generic;
using System.Linq;
using NHibernate;
using TrueOrFalse.Search;

namespace TrueOrFalse
{
    public class SetRepository : RepositoryDbBase<Set>
    {
        private readonly SearchIndexSet _searchIndexSet;

        public SetRepository(
            ISession session, 
            SearchIndexSet searchIndexSet)
            : base(session)
        {
            _searchIndexSet = searchIndexSet;
        }

        public override void Update(Set set)
        {
            ThrowIfNot_IsUserOrAdmin(set.Id);

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

        public override IList<Set> GetByIds(params int[] setIds)
        {
            var sets = base.GetByIds(setIds);
            return setIds.Select(t => sets.First(s => s.Id == t)).ToList();
        }

        public IList<Set>GetForCategory(int categoryId)
        {
            return _session.QueryOver<Set>()
                .JoinQueryOver<Category>(q => q.Categories)
                .Where(c => c.Id == categoryId)
                .List<Set>();
        }

        public override void Delete(Set set)
        {
            ThrowIfNot_IsUserOrAdmin(set.Id);

            base.Delete(set);
            Flush();
        }
    }
}
