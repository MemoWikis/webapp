﻿using System.Collections.Generic;
using Seedworks.Lib.Persistence;
using SolrNet.Impl;

namespace TrueOrFalse.Search
{
    public class MeiliSearchUsersResult : ISearchUsersResult
    {
        public int Count { get; set; }

        public List<int> UserIds { get; set; } = new();

        public IList<User> GetUsers() => Sl.UserRepo.GetByIds(UserIds); //todo: Get From EntityCache
    }
}