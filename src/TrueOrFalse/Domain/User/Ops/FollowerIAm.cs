﻿using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;

public class FollowerIAm : IRegisterAsInstancePerLifetime
{
    private bool _initialized;

    private IList<int> _whoIFollow = new List<int>();

    public FollowerIAm Init(IEnumerable<int> userIds, int myUserId)
    {
        _initialized = true;

        if (!userIds.Any())
            return this;

        var query = @"
            SELECT User_id
            FROM user_to_follower
            WHERE User_id IN ({0})
            AND Follower_id = {1}
        ";

        query = String.Format(query,
            userIds
                .Select(u => u.ToString())
                .Aggregate((a, b) => a + "," + b),
            myUserId);

        _whoIFollow = Sl.R<ISession>()
            .CreateSQLQuery(query)
            .List<int>();

        return this;
    }

    public bool Of(int userId)
    {
        if (!_initialized)
            throw new Exception("call Init(IList<int> userIds) first");

        return _whoIFollow.Any(x => x == userId);
    }
}