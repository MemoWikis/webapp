using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using static CategoryCacheItem;

namespace VueApp;

public class FeedController(
    PermissionCheck _permissionCheck,
    SessionUser _sessionUser) : Controller
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
    public record struct TopicFeedItem(DateTime Date, CategoryChangeType Type, int CategoryChangeId, int TopicId, CategoryVisibility Visibility, string AuthorName);
    public record struct QuestionFeedItem(DateTime Date, QuestionChangeType Type, int QuestionChangeId, int QuestionId, QuestionVisibility Visibility, string AuthorName);

    private FeedItem ToFeedItem(CategoryCacheItem.FeedItem feedItem)
    {
        if (feedItem.CategoryChangeCacheItem != null)
        {
            var change = feedItem.CategoryChangeCacheItem;
            var authorName = change.Author().Name != null ? change.Author().Name : "Unbekannt";
            var topicFeedItem = new TopicFeedItem(
                Date: change.DateCreated,
                Type: change.Type,
                CategoryChangeId: change.Id,
                TopicId: change.CategoryId,
                Visibility: change.Visibility,
                AuthorName: authorName);

            return new FeedItem(feedItem.DateCreated, FeedType.Topic, topicFeedItem, QuestionFeedItem: null);
        }

        if (feedItem.QuestionChangeCacheItem != null)
        {
            var change = feedItem.QuestionChangeCacheItem;
            var authorName = change.Author().Name != null ? change.Author().Name : "Unbekannt";

            var questionFeedItem = new QuestionFeedItem(
                Date: change.DateCreated,
                Type: change.Type,
                QuestionChangeId: change.Id,
                QuestionId: change.QuestionId,
                Visibility: change.Visibility,
                AuthorName: authorName);

            return new FeedItem(feedItem.DateCreated, FeedType.Question, TopicFeedItem: null, questionFeedItem);
        }

        throw new Exception("no valid changeItem");
    }

}