﻿using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;

public class FollowerCounts : IRegisterAsInstancePerLifetime
{
    private readonly Dictionary<int, int> _followers = new Dictionary<int, int>();
    private bool _loaded;

    public FollowerCounts Load(IEnumerable<int> userIds)
    {
        var query = @"
            SELECT Count(User_id), User_id
            FROM user_to_follower
            WHERE User_id IN ({0})
            GROUP BY User_id
        ";

        query = String.Format(query,
            userIds
                .Select(u => u.ToString())
                .Aggregate((a, b) => a + "," + b));

        var listOfObjects = Sl.R<ISession>()
            .CreateSQLQuery(query)
            .List<object[]>();

        foreach (var item in listOfObjects)
            _followers.Add(Convert.ToInt32(item[1]), Convert.ToInt32(item[0]));

        _loaded = true;

        return this;
    }

    public int ByUserId(int userId)
    {
        if(!_loaded)
            throw new Exception("call Load(IList<int> userIds) first");

        if (!_followers.ContainsKey(userId))
            return 0;

        return _followers[userId];
    }
}