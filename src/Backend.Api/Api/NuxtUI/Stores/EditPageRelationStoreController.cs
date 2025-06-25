using Microsoft.AspNetCore.Hosting;
using System.Security;
using Exception = System.Exception;

public class EditPageRelationStoreController(
    SessionUser _sessionUser,
    IHttpContextAccessor _httpContextAccessor,
    PermissionCheck _permissionCheck,
    PageRepository pageRepository,
    PageRelationRepo pageRelationRepo,
    UserWritingRepo _userWritingRepo,
    IWebHostEnvironment _webHostEnvironment,
    SearchResultBuilder _searchResultBuilder) : ApiBaseController
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
        if (GraphService.IsCircularReference(parentPageId: id, childPageId: _sessionUser.FirstWikiId()))
            return new GetPersonalWikiDataResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Page.LoopLink
            };

        var personalWiki = _sessionUser.User.FirstWiki();
        var personalWikiItem = _searchResultBuilder.FillSearchPageItem(personalWiki, _sessionUser.UserId);
        var recentlyUsedRelationTargetPages = new List<SearchPageItem>();

        if (_sessionUser.User.RecentlyUsedRelationTargetPageIds.Count > 0)
        {
            foreach (var pageId in _sessionUser.User.RecentlyUsedRelationTargetPageIds)
            {
                var pageCacheItem = EntityCache.GetPage(pageId);
                recentlyUsedRelationTargetPages.Add(
                    _searchResultBuilder.FillSearchPageItem(pageCacheItem, _sessionUser.UserId)
                );
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

        return new RemovePagesResult { Success = true, Data = removedChildrenIds };
    }

    public enum TargetPosition
    {
        Before,
        After,
        Inner,
        None
    }

    public readonly record struct MovePageRequest(
        int MovingPageId,
        int TargetId,
        TargetPosition Position,
        int NewParentId,
        int OldParentId);

    public readonly record struct TryMovePageResult(
        int OldParentId,
        int NewParentId,
        MovePageRequest UndoMove);

    private TryMovePageResult TryMovePage(MovePageRequest request)
    {
        if (!_sessionUser.IsLoggedIn)
            throw new SecurityException(FrontendMessageKeys.Error.User.NotLoggedIn);

        if (!_permissionCheck.CanMovePage(request.MovingPageId, request.OldParentId, request.NewParentId))
        {
            if (request.NewParentId == FeaturedPage.RootPageId &&
                EntityCache.GetPage(request.MovingPageId)?.Visibility == PageVisibility.Public)
                throw new SecurityException(FrontendMessageKeys.Error.Page.ParentIsRoot);

            throw new SecurityException(FrontendMessageKeys.Error.Page.MissingRights);
        }

        if (request.MovingPageId == request.NewParentId)
            throw new InvalidOperationException(
                FrontendMessageKeys.Error.Page.CircularReference);

        var relationToMove = EntityCache.GetPage(request.OldParentId)?.ChildRelations
            .FirstOrDefault(r => r.ChildId == request.MovingPageId);

        if (relationToMove == null)
        {
            Log.Error(
                "PageRelations - MovePage: no relation found to move - movingPageId:{0}, parentId:{1}",
                request.MovingPageId, request.OldParentId);
            throw new InvalidOperationException(FrontendMessageKeys.Error.Default);
        }

        var undoMovePageData = GetUndoMovePageData(relationToMove, request.NewParentId, request.TargetId);

        var modifyRelationsForPage = new ModifyRelationsForPage(pageRepository, pageRelationRepo);

        if (request.Position == TargetPosition.Inner)
        {
            PageOrderer.MoveIn(relationToMove, request.TargetId, _sessionUser.UserId,
                modifyRelationsForPage, _permissionCheck);

            return new TryMovePageResult(request.OldParentId, request.TargetId, undoMovePageData);
        }

        if (request.Position == TargetPosition.Before)
            PageOrderer.MoveBefore(relationToMove, request.TargetId, request.NewParentId,
                _sessionUser.UserId, modifyRelationsForPage);
        else if (request.Position == TargetPosition.After)
            PageOrderer.MoveAfter(relationToMove, request.TargetId, request.NewParentId,
                _sessionUser.UserId, modifyRelationsForPage);
        else if (request.Position == TargetPosition.None)
            throw new InvalidOperationException(FrontendMessageKeys.Error.Default);

        return new TryMovePageResult(request.OldParentId, request.NewParentId, undoMovePageData);
    }

    public readonly record struct MovePageResult(
        bool Success,
        TryMovePageResult? Data,
        string Error);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public MovePageResult MovePage([FromBody] MovePageRequest request)
    {
        try
        {
            var result = TryMovePage(request);
            return new MovePageResult(true, result, "");
        }
        catch (Exception ex)
        {
            return new MovePageResult(false, null, ex.Message);
        }
    }

    private MovePageRequest GetUndoMovePageData(
        PageRelationCache relation,
        int newParentId,
        int targetId)
    {
        if (relation.PreviousId != null &&
            _permissionCheck.CanViewPage((int)relation.PreviousId))
            return new MovePageRequest(relation.ChildId, (int)relation.PreviousId,
                TargetPosition.After, relation.ParentId,
                newParentId);

        if (relation.NextId != null && _permissionCheck.CanViewPage((int)relation.NextId))
            return new MovePageRequest(relation.ChildId, (int)relation.NextId, TargetPosition.Before,
                relation.ParentId,
                newParentId);

        return new MovePageRequest(relation.ChildId, relation.ParentId, TargetPosition.Inner,
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
        var personalWiki = _sessionUser.User.FirstWiki();

        if (personalWiki == null)
            return new PersonalWikiResult { Success = false, MessageKey = FrontendMessageKeys.Error.Default };

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
        var personalWiki = _sessionUser.User.FirstWiki();

        if (personalWiki == null)
            return new RemoveFromPersonalWikiResult { Success = false, MessageKey = FrontendMessageKeys.Error.Default };

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