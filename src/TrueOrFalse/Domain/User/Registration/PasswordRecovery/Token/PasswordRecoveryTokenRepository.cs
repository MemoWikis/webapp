using NHibernate;
using Seedworks.Lib.Persistence;

public class PasswordRecoveryTokenRepository : RepositoryDb<PasswordRecoveryToken>
{
    public PasswordRecoveryTokenRepository(ISession session) : base(session) { }
}