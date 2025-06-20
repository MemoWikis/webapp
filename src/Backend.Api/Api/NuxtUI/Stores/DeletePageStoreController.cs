using static PageDeleter;

public class DeletePageStoreController(
    SessionUser _sessionUser,
    PageDeleter _pageDeleter,
    CrumbtrailService _crumbtrailService,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    IHttpContextAccessor _httpContextAccessor,
    QuestionReadingRepo _questionReadingRepo,
    PermissionCheck _permissionCheck) : ApiBaseController
{
    public record struct SuggestedNewParent(
        int Id,
        string Name,
        int QuestionCount,
        string ImageUrl,
        string MiniImageUrl,
        PageVisibility Visibility);

    private SuggestedNewParent FillSuggestedNewParent(PageCacheItem page)
    {
        return new SuggestedNewParent
        {
            Id = page.Id,
            Name = page.Name,
            QuestionCount = page.GetCountQuestionsAggregated(_sessionUser.UserId),
            ImageUrl = new PageImageSettings(page.Id, _httpContextAccessor)
                .GetUrl_128px(asSquare: true).Url,
            MiniImageUrl = new ImageFrontendData(
                    _imageMetaDataReadingRepo.GetBy(page.Id, ImageType.Page),
                    _httpContextAccessor,
                    _questionReadingRepo)
                .GetImageUrl(30, true, false, ImageType.Page)
                .Url,
            Visibility = page.Visibility
        };
    }

    public readonly record struct DeleteData(
        string Name,
        bool WouldHaveOrphanedChildren,
        SuggestedNewParent? SuggestedNewParent,
        bool HasQuestion,
        bool HasPublicQuestion,
        bool IsWiki);

    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public DeleteData GetDeleteData([FromRoute] int id)
    {
        var page = EntityCache.GetPage(id);
        var wouldHaveOrphanedChildren = !CanDeleteItemBasedOnChildParentCount(id, _sessionUser.UserId);
        if (page == null)
            throw new Exception(
                "Page couldn't be deleted. Page with specified Id cannot be found.");

        var questions = EntityCache
            .GetPage(id)?
            .GetAggregatedQuestions(
                _sessionUser.UserId,
                onlyVisible: false,
                permissionCheck: _permissionCheck
            );

        var hasQuestion = questions?.Count > 0;

        if (!wouldHaveOrphanedChildren && !hasQuestion)
            return new DeleteData(page.Name, WouldHaveOrphanedChildren: false, SuggestedNewParent: null,
                HasQuestion: false, HasPublicQuestion: false, IsWiki: page.IsWiki);

        var hasPublicQuestion = questions?
            .Any(q => q.Visibility == QuestionVisibility.Public) ?? false;

        var currentWiki = EntityCache.GetPage(_sessionUser.CurrentWikiId);

        var parents = _crumbtrailService.BuildCrumbtrail(page, currentWiki);
        var newParentId = _crumbtrailService.SuggestNewParent(parents, _sessionUser, hasPublicQuestion);

        if (newParentId == null)
            return new DeleteData(page.Name, wouldHaveOrphanedChildren, SuggestedNewParent: null, hasQuestion,
                hasPublicQuestion, IsWiki: page.IsWiki);

        var suggestedNewParent = FillSuggestedNewParent(EntityCache.GetPage((int)newParentId));

        return new DeleteData(page.Name, wouldHaveOrphanedChildren, suggestedNewParent, hasQuestion, hasPublicQuestion,
            IsWiki: page.IsWiki);
    }

    public readonly record struct DeleteRequest(int PageToDeleteId, int? ParentForQuestionsId);

    public record struct DeleteResponse(
        bool Success,
        bool? HasChildren = null,
        RedirectPage? RedirectParent = null,
        string? MessageKey = null);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public DeleteResponse Delete([FromBody] DeleteRequest deleteRequest)
    {
        if (EntityCache.PageHasQuestion(deleteRequest.PageToDeleteId))
        {
            if (deleteRequest.ParentForQuestionsId == 0)
                return new DeleteResponse(Success: false, MessageKey: FrontendMessageKeys.Error.Page.PageNotSelected);

            if (deleteRequest.ParentForQuestionsId == deleteRequest.PageToDeleteId)
                return new DeleteResponse(Success: false,
                    MessageKey: FrontendMessageKeys.Error.Page.NewPageIdIsPageIdToBeDeleted);
        }

        var deleteResult = _pageDeleter.DeletePage(deleteRequest.PageToDeleteId, deleteRequest.ParentForQuestionsId);

        return new DeleteResponse(
            Success: deleteResult.Success,
            HasChildren: deleteResult.HasChildren,
            RedirectParent: deleteResult.RedirectParent,
            MessageKey: deleteResult.MessageKey);
    }

    private bool CanDeleteItemBasedOnChildParentCount(
        int pageId,
        int userId)
    {
        var allChildren = GraphService.Children(pageId);
        var visitedChildrenIds = new HashSet<int>();

        if (allChildren.Count > 0)
        {
            var allVisibleChildren =
                GraphService.VisibleChildren(pageId, _permissionCheck, userId);

            if (allVisibleChildren.Count > 0)
            {
                foreach (var child in allVisibleChildren)
                {
                    visitedChildrenIds.Add(child.Id);

                    var visibleParents = GraphService
                        .VisibleParents(child.Id, _permissionCheck)
                        .Where(p => p.Id != pageId)
                        .ToList();

                    if (visibleParents.Count < 1)
                        return false;
                }
            }

            var privateChildren =
                allChildren
                    .Where(c => !visitedChildrenIds.Contains(c.Id))
                    .ToList();

            if (privateChildren.Count > 0)
            {
                foreach (var child in privateChildren)
                {
                    var hasExtraParent = GraphService.ParentIds(child).Any(i => i != pageId);

                    if (!hasExtraParent)
                        return false;
                }
            }
        }

        return true;
    }
}