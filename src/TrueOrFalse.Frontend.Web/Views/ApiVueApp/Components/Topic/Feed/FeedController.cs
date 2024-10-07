using JetBrains.Annotations;
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
    IHttpContextAccessor _httpContextAccessor) : Controller
{
    public readonly record struct GetFeedResponse(IList<FeedItem> feedItems, int maxCount);
    public record struct FeedItem(DateTime Date, FeedType Type, TopicFeedItem? TopicFeedItem, QuestionFeedItem? QuestionFeedItem, Author Author);
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

    public record struct TopicFeedItem(DateTime Date, CategoryChangeType Type, int CategoryChangeId, int TopicId, string Title, CategoryVisibility Visibility, Author Author, NameChange? NameChange = null);
    public record struct QuestionFeedItem(DateTime Date, QuestionChangeType Type, int QuestionChangeId, int QuestionId, string Text, QuestionVisibility Visibility, Author Author);

    public record struct Author(string Name = "Unbekannt", int Id = -1, string ImageUrl = "");
    private FeedItem ToFeedItem(CategoryCacheItem.FeedItem feedItem)
    {
        if (feedItem.CategoryChangeCacheItem != null)
        {
            var change = feedItem.CategoryChangeCacheItem;
            var author = SetAuthor(change.Author());
            var cachedNameChange = change.CategoryChangeRecord?.NameChange;

            var nameChange = change.Type == CategoryChangeType.Renamed ? cachedNameChange : null;

            var topicFeedItem = new TopicFeedItem(
                Date: change.DateCreated,
                Type: change.Type,
                CategoryChangeId: change.Id,
                TopicId: change.CategoryId,
                Title: change.Category.Name,
                Visibility: change.Visibility,
                Author: author,
                NameChange: nameChange);

            return new FeedItem(feedItem.DateCreated, FeedType.Topic, topicFeedItem, QuestionFeedItem: null, Author: author);
        }

        if (feedItem.QuestionChangeCacheItem != null)
        {
            var change = feedItem.QuestionChangeCacheItem;
            var author = SetAuthor(change.Author());

            var questionFeedItem = new QuestionFeedItem(
                Date: change.DateCreated,
                Type: change.Type,
                QuestionChangeId: change.Id,
                QuestionId: change.QuestionId,
                Text: change.Question.GetShortTitle(),
                Visibility: change.Visibility,
                Author: author);

            return new FeedItem(feedItem.DateCreated, FeedType.Question, TopicFeedItem: null, questionFeedItem, Author: author);
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

    public readonly record struct GetChangeRequest(int id, int changeId);

    [HttpPost]
    public void GetTopicChange([FromBody] GetChangeRequest req)
    {
        var topic = EntityCache.GetCategory(req.id);
        if (!_permissionCheck.CanView(topic))
        {
            return;
        }
    }

    [HttpPost]
    public void GetQuestionChange([FromBody] GetChangeRequest req)
    {
        var question = EntityCache.GetQuestion(req.id);
        if (!_permissionCheck.CanView(question))
        {
            return;
        }
    }
}