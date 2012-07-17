using NHibernate;

namespace TrueOrFalse.Core
{
    public class GetTotalQuestionCount : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;

        public GetTotalQuestionCount(ISession session){
            _session = session;
        }

        public int Run()
        {
            return _session.CreateSQLQuery("SELECT Count(Id) FROM Question").UniqueResult<int>();
        }
    }
}
