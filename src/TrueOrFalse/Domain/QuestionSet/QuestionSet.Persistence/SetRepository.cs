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
        }

        public override void Create(Set set)
        {
            base.Create(set);
            _searchIndexSet.Update(set);
        }

        public override void Delete(int id)
        {
            var set = GetById(id);
            _searchIndexSet.Delete(set);
            base.Delete(id);
        }
    }
}
