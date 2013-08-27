using NHibernate;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;

namespace TrueOrFalse
{
    public class SetRepository : RepositoryDb<Set>
    {
        private readonly SetIndex _setIndex;

        public SetRepository(ISession session, SetIndex setIndex)
            : base(session)
        {
            _setIndex = setIndex;
        }

        public override void Update(Set set)
        {
            _setIndex.Update(set);
            base.Update(set);
        }

        public override void Create(Set set)
        {
            base.Create(set);
            _setIndex.Update(set);
        }

        public override void Delete(int id)
        {
            var set = GetById(id);
            _setIndex.Delete(set);
            base.Delete(id);
        }
    }
}
