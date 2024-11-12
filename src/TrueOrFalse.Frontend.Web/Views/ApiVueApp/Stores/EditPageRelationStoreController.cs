using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using Exception = System.Exception;

namespace VueApp;

public class EditPageRelationStoreController(
    SessionUser _sessionUser,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    IHttpContextAccessor _httpContextAccessor,
    QuestionReadingRepo _questionReadingRepo,
    PermissionCheck _permissionCheck,
    PageRepository pageRepository,
    PageRelationRepo pageRelationRepo,
    UserWritingRepo _userWritingRepo,
    IWebHostEnvironment _webHostEnvironment) : Controller
{
    public record struct PersonalWikiData(
        SearchPageItem PersonalWiki,
        SearchPageItem[] RecentlyUsedRelationTargetPages);

    public record struct GetPersonalWikiDataResult(
        bool Success,
        string MessageKey,
        PersonalWikiData? Data);

    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public GetPersonalWikiDataResult GetPersonalWikiData([FromRoute] int id)
    {
        if (GraphService.Descendants(id).Any(c => c.Id == _sessionUser.User.StartPageId))
            return new GetPersonalWikiDataResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Page.LoopLink
            };

        var personalWiki = EntityCache.GetPage(_sessionUser.User.StartPageId);
        var personalWikiItem = new SearchHelper(_imageMetaDataReadingRepo,
                _httpContextAccessor,
                _questionReadingRepo)
            .FillSearchPageItem(personalWiki, _sessionUser.UserId);
        var recentlyUsedRelationTargetPages = new List<SearchPageItem>();

        if (_sessionUser.User.RecentlyUsedRelationTargetPageIds.Count > 0)
        {
            foreach (var pageId in _sessionUser.User.RecentlyUsedRelationTargetPageIds)
            {
                var topicCacheItem = EntityCache.GetPage(pageId);
                recentlyUsedRelationTargetPages.Add(new SearchHelper(_imageMetaDataReadingRepo,
                        _httpContextAccessor,
                        _questionReadingRepo)
                    .FillSearchPageItem(topicCacheItem, _sessionUser.UserId));
            }
        }

        return new GetPersonalWikiDataResult
        {
            Success = true,
            Data = new PersonalWikiData
            {
                PersonalWiki = personalWikiItem,
                RecentlyUsedRelationTargetPages = recentlyUsedRelationTargetPages.ToArray()
            }
        };
    }

    public readonly record struct RemovePagesJson(int parentId, int[] childIds);

    public readonly record struct RemovePagesResult(bool Success, List<int> Data);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public RemovePagesResult RemovePages([FromBody] RemovePagesJson json)
    {
        var removedChildrenIds = new List<int>();

        foreach (var childId in json.childIds)
        {
            var result = new ChildModifier(
                _permissionCheck,
                _sessionUser,
                pageRepository,
                _userWritingRepo,
                _httpContextAccessor,
                _webHostEnvironment,
                pageRelationRepo).RemoveParent(json.parentId, childId);
            if (result.Success)
                removedChildrenIds.Add(childId);
        }

        return new RemovePagesResult
        {
            Success = true,
            Data = removedChildrenIds
        };
    }

    public enum TargetPosition
    {
        Before,
        After,
        Inner,
        None
    }

    public readonly record struct MovePageJson(
        int MovingPageId,
        int TargetId,
        TargetPosition Position,
        int NewParentId,
        int OldParentId);

    public readonly record struct TryMovePageResult(
        int OldParentId,
        int NewParentId,
        MovePageJson UndoMove);

    private TryMovePageResult TryMovePage(MovePageJson json)
    {
        if (!_sessionUser.IsLoggedIn)
            throw new SecurityException(FrontendMessageKeys.Error.User.NotLoggedIn);

        if (!_permissionCheck.CanMovePage(json.MovingPageId, json.OldParentId, json.NewParentId))
        {
            if (json.NewParentId == RootPage.RootCategoryId && EntityCache.GetPage(json.MovingPageId)?.Visibility == PageVisibility.All)
                throw new SecurityException(FrontendMessageKeys.Error.Page.ParentIsRoot);

            throw new SecurityException(FrontendMessageKeys.Error.Page.MissingRights);
        }

        if (json.MovingPageId == json.NewParentId)
            throw new InvalidOperationException(
                FrontendMessageKeys.Error.Page.CircularReference);

        var relationToMove = EntityCache.GetPage(json.OldParentId)?.ChildRelations
            .FirstOrDefault(r => r.ChildId == json.MovingPageId);

        if (relationToMove == null)
        {
            Logg.r.Error(
                "CategoryRelations - MovePage: no relation found to move - movingPageId:{0}, parentId:{1}",
                json.MovingPageId, json.OldParentId);
            throw new InvalidOperationException(FrontendMessageKeys.Error.Default);
        }

        var undoMovePageData =
            GetUndoMovePageData(relationToMove, json.NewParentId, json.TargetId);

        var modifyRelationsForCategory =
            new ModifyRelationsForPage(pageRepository, pageRelationRepo);

        if (json.Position == TargetPosition.Before)
            PageOrderer.MoveBefore(relationToMove, json.TargetId, json.NewParentId,
                _sessionUser.UserId, modifyRelationsForCategory);
        else if (json.Position == TargetPosition.After)
            PageOrderer.MoveAfter(relationToMove, json.TargetId, json.NewParentId,
                _sessionUser.UserId, modifyRelationsForCategory);
        else if (json.Position == TargetPosition.Inner)
            PageOrderer.MoveIn(relationToMove, json.TargetId, _sessionUser.UserId,
                modifyRelationsForCategory, _permissionCheck);
        else if (json.Position == TargetPosition.None)
            throw new InvalidOperationException(FrontendMessageKeys.Error.Default);

        return new TryMovePageResult(json.OldParentId, json.NewParentId, undoMovePageData);
    }

    public readonly record struct MovePageResult(
        bool Success,
        TryMovePageResult? Data,
        string Error);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public MovePageResult MovePage([FromBody] MovePageJson json)
    {
        try
        {
            var result = TryMovePage(json);
            return new MovePageResult(true, result, "");
        }
        catch (Exception ex)
        {
            return new MovePageResult(false, null, ex.Message);
        }
    }

    private MovePageJson GetUndoMovePageData(
        PageRelationCache relation,
        int newParentId,
        int targetId)
    {
        if (relation.PreviousId != null &&
            _permissionCheck.CanViewPage((int)relation.PreviousId))
            return new MovePageJson(relation.ChildId, (int)relation.PreviousId,
                TargetPosition.After, relation.ParentId,
                newParentId);

        if (relation.NextId != null && _permissionCheck.CanViewPage((int)relation.NextId))
            return new MovePageJson(relation.ChildId, (int)relation.NextId, TargetPosition.Before,
                relation.ParentId,
                newParentId);

        return new MovePageJson(relation.ChildId, relation.ParentId, TargetPosition.Inner,
            targetId, targetId);
    }

    public readonly record struct PersonalWikiResult(
        bool Success,
        ChildModifier.AddChildResult Data,
        string MessageKey);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public PersonalWikiResult AddToPersonalWiki([FromRoute] int id)
    {
        var personalWiki = EntityCache.GetPage(_sessionUser.User.StartPageId);

        if (personalWiki == null)
            return new PersonalWikiResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Default
            };

        if (personalWiki.ChildRelations.Any(r => r.ChildId == id))
        {
            return new PersonalWikiResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Page.IsAlreadyLinkedAsChild
            };
        }

        return new PersonalWikiResult
        {
            Success = true,
            Data = new ChildModifier(_permissionCheck,
                    _sessionUser,
                    pageRepository,
                    _userWritingRepo,
                    _httpContextAccessor,
                    _webHostEnvironment,
                    pageRelationRepo)
                .AddChild(id, personalWiki.Id)
        };
    }
    public readonly record struct RemoveFromPersonalWikiResult(
        bool Success,
        ChildModifier.RemoveParentResult Data,
        string MessageKey);
    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public RemoveFromPersonalWikiResult RemoveFromPersonalWiki([FromRoute] int id)
    {
        var personalWiki = EntityCache.GetPage(_sessionUser.User.StartPageId);

        if (personalWiki == null)
            return new RemoveFromPersonalWikiResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Default
            };

        if (personalWiki.ChildRelations.Any(r => r.ChildId != id))
        {
            return new RemoveFromPersonalWikiResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Page.IsNotAChild
            };
        }

        return new RemoveFromPersonalWikiResult
        {
            Success = true,
            Data = new ChildModifier(_permissionCheck,
                    _sessionUser,
                    pageRepository,
                    _userWritingRepo,
                    _httpContextAccessor,
                    _webHostEnvironment,
                    pageRelationRepo)
                .RemoveParent(personalWiki.Id, id)
        };
    }
}