using NHibernate;

public class PasswordRecoveryTokenValidator(ISession session) : IRegisterAsInstancePerLifetime
{
    public virtual PasswordRecoveryToken? Run(string token)
    {
        var passwordToken = session.QueryOver<PasswordRecoveryToken>()
            .Where(x => x.Token == token)
            .SingleOrDefault<PasswordRecoveryToken>();

        return passwordToken;
    }
}