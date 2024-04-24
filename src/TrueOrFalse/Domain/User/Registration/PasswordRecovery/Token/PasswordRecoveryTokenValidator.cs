using NHibernate;

public class PasswordRecoveryTokenValidator : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public PasswordRecoveryTokenValidator(ISession session)
    {
        _session = session;
    }

    public virtual PasswordRecoveryToken? Run(string token)
    {
        var passwordToken = _session.QueryOver<PasswordRecoveryToken>()
            .Where(x => x.Token == token)
            .SingleOrDefault<PasswordRecoveryToken>();

        return passwordToken;
    }
}