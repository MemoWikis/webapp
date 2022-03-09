﻿using System.Collections.Generic;
using System.Linq;

public class AuthorViewModel
{
    public string Name;
    public string ImageUrl;
    public UserTinyModel User;
    public int Reputation;
    public int ReputationPos;
    public int Id;

    public static List<AuthorViewModel> Convert(IList<UserTinyModel> authors)
    {
        return authors.Select(author => new AuthorViewModel
        {
            ImageUrl = new UserImageSettings(author.Id).GetUrl_20px(author).Url,
            User = author,
            Reputation = author.Reputation,
            Name = author.Name,
            Id = author.Id,
            ReputationPos = author.ReputationPos
        }).ToList();
    }
    public static List<AuthorViewModel> Convert(IList<AuthorCacheItem> authors)
    {
        return authors.Select(author => new AuthorViewModel
        {
            ImageUrl = new UserImageSettings(author.Id).GetUrl_20px(author).Url,
            Reputation = author.Reputation,
            ReputationPos = author.ReputationPos,
            Name = author.Name,
            Id = author.Id
        }).ToList();
    }

}