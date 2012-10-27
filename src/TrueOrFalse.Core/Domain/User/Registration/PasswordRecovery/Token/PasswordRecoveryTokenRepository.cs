using NHibernate;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core.Registration
{
    public class PasswordRecoveryTokenRepository : RepositoryDb<PasswordRecoveryToken>
    {
        public PasswordRecoveryTokenRepository(ISession session) : base(session) { }
    }
}
