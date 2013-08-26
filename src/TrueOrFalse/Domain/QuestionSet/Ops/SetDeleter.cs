using System.Linq;
using NHibernate;

namespace TrueOrFalse
{
    public class SetDeleter : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;
        private readonly SetRepository _setRepo;

        public SetDeleter(ISession session, SetRepository setRepo)
        {
            _session = session;
            _setRepo = setRepo;
        }

        public void Run(int setId)
        {
            var set = _setRepo.GetById(setId);
            _setRepo.Delete(set);
        }
    }
}
