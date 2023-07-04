﻿using NHibernate;

public class PasswordRecoveryTokenValidator : IRegisterAsInstancePerLifetime
{
    public virtual PasswordRecoveryToken Run(string token)
    {
        var passwordToken = Sl.R<ISession>().QueryOver<PasswordRecoveryToken>()
            .Where(x => x.Token == token)
            .SingleOrDefault<PasswordRecoveryToken>();

        return passwordToken;
    }
}