﻿using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using static CategoryCacheItem;

namespace VueApp;

public class FeedController(
    PermissionCheck _permissionCheck,
    SessionUser _sessionUser,
    HttpContextAccessor _httpContextAccessor) : Controller
{
    public readonly record struct GetFeedResponse(IList<FeedItem> feedItems, int maxCount);
    public record struct FeedItem(DateTime Date, FeedType Type, TopicFeedItem? TopicFeedItem, QuestionFeedItem? QuestionFeedItem);
    public readonly record struct GetFeedRequest(int TopicId, int Page, int PageSize, bool GetDescendants = true, bool GetQuestions = true);

    [HttpPost]
    public GetFeedResponse Get([FromBody] GetFeedRequest req)
    {
        var topic = EntityCache.GetCategory(req.TopicId);

        var (pagedChanges, maxCount) = topic.GetVisibleFeedItemsByPage(_permissionCheck, _sessionUser.UserId, req.Page, req.PageSize, req.GetDescendants, req.GetQuestions);

        return new GetFeedResponse(
            feedItems: pagedChanges.Select(ToFeedItem).ToList(),
            maxCount: maxCount);
    }
    public record struct TopicFeedItem(DateTime Date, CategoryChangeType Type, int CategoryChangeId, int TopicId, CategoryVisibility Visibility, Author Author);
    public record struct QuestionFeedItem(DateTime Date, QuestionChangeType Type, int QuestionChangeId, int QuestionId, QuestionVisibility Visibility, Author Author);

    public record struct Author(string Name = "Unbekannt", int? Id = null, string ImageUrl = "");
    private FeedItem ToFeedItem(CategoryCacheItem.FeedItem feedItem)
    {
        if (feedItem.CategoryChangeCacheItem != null)
        {
            var change = feedItem.CategoryChangeCacheItem;

            var topicFeedItem = new TopicFeedItem(
                Date: change.DateCreated,
                Type: change.Type,
                CategoryChangeId: change.Id,
                TopicId: change.CategoryId,
                Visibility: change.Visibility,
                Author: SetAuthor(change.Author()));

            return new FeedItem(feedItem.DateCreated, FeedType.Topic, topicFeedItem, QuestionFeedItem: null);
        }

        if (feedItem.QuestionChangeCacheItem != null)
        {
            var change = feedItem.QuestionChangeCacheItem;

            var questionFeedItem = new QuestionFeedItem(
                Date: change.DateCreated,
                Type: change.Type,
                QuestionChangeId: change.Id,
                QuestionId: change.QuestionId,
                Visibility: change.Visibility,
                Author: SetAuthor(change.Author()));

            return new FeedItem(feedItem.DateCreated, FeedType.Question, TopicFeedItem: null, questionFeedItem);
        }

        throw new Exception("no valid changeItem");
    }

    private Author SetAuthor([CanBeNull] UserCacheItem user)
    {
        var author = new Author();
        if (user != null)
        {
            author.Name = user.Name;
            author.Id = user.Id;
            author.ImageUrl = new UserImageSettings(user.Id, _httpContextAccessor)
                .GetUrl_128px_square(user)
                .Url;
        }

        return author;
    }

}