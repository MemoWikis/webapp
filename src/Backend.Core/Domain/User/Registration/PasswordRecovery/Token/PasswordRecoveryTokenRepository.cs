using NHibernate;

public class PasswordRecoveryTokenRepository : RepositoryDb<PasswordRecoveryToken>
{
    public PasswordRecoveryTokenRepository(ISession session) : base(session) { }
}