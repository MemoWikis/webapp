﻿using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs245
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"DROP TABLE answer_test;"
            ).ExecuteUpdate();
    }
}