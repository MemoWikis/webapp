using System.Linq;
using NHibernate;
using TrueOrFalse.Search;

namespace TrueOrFalse
{
    public class SetDeleter : IRegisterAsInstancePerLifetime
    {
        private readonly SetRepository _setRepo;
        private readonly SearchIndexSet _searchIndexSet;

        public SetDeleter(SetRepository setRepo, SearchIndexSet searchIndexSet)
        {
            _setRepo = setRepo;
            _searchIndexSet = searchIndexSet;
        }

        public void Run(int setId)
        {
            var set = _setRepo.GetById(setId);
            _searchIndexSet.Delete(set);
            _setRepo.Delete(set);
        }
    }
}
