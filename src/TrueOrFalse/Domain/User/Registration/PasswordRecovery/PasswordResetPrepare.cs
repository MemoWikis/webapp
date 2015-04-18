using System;
using NHibernate;

public class PasswordResetPrepare : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public PasswordResetPrepare(ISession session){
        _session = session;
    }

    public PasswordResetPrepareResult Run(string token)
    {
        var passwortToken = _session.QueryOver<PasswordRecoveryToken>()
                .Where(x => x.Token == token)
                .SingleOrDefault<PasswordRecoveryToken>();

        if(passwortToken == null)
            return new PasswordResetPrepareResult { NoTokenFound = true };

        if((DateTime.Now - passwortToken.DateCreated).TotalDays > 3)
            return new PasswordResetPrepareResult { TokenOlderThan72h = true };

        return new PasswordResetPrepareResult { Email = passwortToken.Email, Success = true };
    }
}