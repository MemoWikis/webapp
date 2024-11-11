using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using static PageCacheItem;

namespace VueApp;

public class FeedController(
    PermissionCheck _permissionCheck,
    SessionUser _sessionUser,
    IHttpContextAccessor _httpContextAccessor,
    CommentRepository _commentRepository) : Controller
{
    public readonly record struct GetFeedResponse(IList<FeedItem> feedItems, int maxCount);
    public record struct FeedItem(DateTime Date, FeedType Type, PageFeedItem? PageFeedItem, QuestionFeedItem? QuestionFeedItem, Author Author);
    public readonly record struct GetFeedRequest(int PageId, int Page, int PageSize, bool GetDescendants = true, bool GetQuestions = true, bool GetItemsInGroups = false);

    [HttpPost]
    public GetFeedResponse Get([FromBody] GetFeedRequest req)
    {
        var topic = EntityCache.GetPage(req.PageId);

        var (pagedChanges, maxCount) = topic.GetVisibleFeedItemsByPage(_permissionCheck, _sessionUser.UserId, req.Page, req.PageSize, req.GetDescendants, req.GetQuestions, req.GetItemsInGroups);

        return new GetFeedResponse(
            feedItems: pagedChanges.Select(ToFeedItem).ToList(),
            maxCount: maxCount);
    }

    public record struct PageFeedItem(
        DateTime Date,
        PageChangeType Type,
        int CategoryChangeId,
        int PageId, string Title,
        PageVisibility Visibility,
        Author Author,
        NameChange? NameChange = null,
        RelationChanges? RelationChanges = null,
        DeleteData? DeleteData = null,
        bool IsGroup = false,
        int? OldestChangeIdInGroup = null);
    public record struct QuestionFeedItem(DateTime Date, QuestionChangeType Type, int QuestionChangeId, int QuestionId, string Text, QuestionVisibility Visibility, Author Author, Comment? Comment);

    public record struct Author(string Name = "Unbekannt", int Id = -1, string ImageUrl = "");
    private FeedItem ToFeedItem(PageCacheItem.FeedItem feedItem)
    {
        if (feedItem.CategoryChangeCacheItem != null)
        {
            var change = feedItem.CategoryChangeCacheItem;
            var author = SetAuthor(change.Author());
            var cachedNameChange = change.PageChangeData.NameChange;

            NameChange? nameChange = change.Type == PageChangeType.Renamed ? cachedNameChange : null;

            var relationChanges = GetRelationChanges(change);
            var deleteData = change.PageChangeData.DeleteData != null ? GetDeleteData(change.Type, change.PageChangeData.DeleteData?.DeletedName, change.PageChangeData.DeleteData?.DeleteChangeId) : null;

            var topicFeedItem = new PageFeedItem(
                Date: change.DateCreated,
                Type: change.Type,
                CategoryChangeId: change.Id,
                PageId: change.PageId,
                Title: change.Page.Name,
                Visibility: change.Visibility,
                Author: author,
                NameChange: nameChange,
                RelationChanges: relationChanges,
                DeleteData: deleteData,
                IsGroup: change.IsGroup,
                OldestChangeIdInGroup: change.IsGroup ? change.GroupedCategoryChangeCacheItems.OrderBy(c => c.DateCreated).First().Id : null);

            return new FeedItem(feedItem.DateCreated, FeedType.Page, topicFeedItem, QuestionFeedItem: null, Author: author);
        }

        if (feedItem.QuestionChangeCacheItem != null)
        {
            var change = feedItem.QuestionChangeCacheItem;
            var author = SetAuthor(change.Author());

            var commentId = change.QuestionChangeData.CommentIdsChange.NewCommentIds?.Except(change.QuestionChangeData.CommentIdsChange.OldCommentIds).ToList().LastOrDefault();

            var comment = commentId != null ? _commentRepository.GetById((int)commentId) : null;

            var commentTitle = comment?.Title?.Length > 0 ? comment.Title : comment?.Text;

            var questionFeedItem = new QuestionFeedItem(
                Date: change.DateCreated,
                Type: change.Type,
                QuestionChangeId: change.Id,
                QuestionId: change.QuestionId,
                Text: change.Question.GetShortTitle(),
                Visibility: change.Visibility,
                Author: author,
                Comment: comment != null ? new Comment(commentTitle, comment.Id) : null);

            return new FeedItem(feedItem.DateCreated, FeedType.Question, PageFeedItem: null, questionFeedItem, Author: author);
        }

        throw new Exception("no valid changeItem");
    }

    public record struct Comment(string Title, int Id);
    public record struct RelatedPage(int Id, string Name);
    public record struct RelationChanges(List<RelatedPage> AddedParents, List<RelatedPage> RemovedParents, List<RelatedPage> AddedChildren, List<RelatedPage> RemovedChildren);
    public record struct DeleteData(int? DeleteChangeId, string DeletedName);

    private List<RelatedPage> GetRelatedPages(IEnumerable<int> ids)
    {
        var relatedPages = new List<RelatedPage>();
        foreach (var id in ids)
        {
            if (_permissionCheck.CanViewPage(id))
            {
                var relatedPage = EntityCache.GetPage(id);
                if (relatedPage != null) relatedPages.Add(new RelatedPage(id, relatedPage.Name));
            }
        }
        return relatedPages;
    }
    private RelationChanges? GetRelationChanges(PageChangeCacheItem change)
    {
        if (change.Type != PageChangeType.Relations && !(change.Type == PageChangeType.Create && change.IsGroup))
            return null;

        var relationChange = change.Type == PageChangeType.Relations ? change.PageChangeData.RelationChange : change.GroupedCategoryChangeCacheItems.Where(c => c.Type == PageChangeType.Relations).MinBy(c => c.DateCreated)?.PageChangeData.RelationChange;

        if (relationChange == null)
            return null;

        var addedParents = GetRelatedPages(relationChange?.AddedParentIds);
        var removedParents = GetRelatedPages(relationChange?.RemovedParentIds);
        var addedChildren = GetRelatedPages(relationChange?.AddedChildIds);
        var removedChildren = GetRelatedPages(relationChange?.RemovedChildIds);

        return new RelationChanges(addedParents, removedParents, addedChildren, removedChildren);
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

    private DeleteData? GetDeleteData(PageChangeType type, [CanBeNull] string deletedName, int? changeId)
    {
        if (type == PageChangeType.ChildPageDeleted || type == PageChangeType.QuestionDeleted)
            return new DeleteData(changeId, deletedName);

        return null;
    }
}