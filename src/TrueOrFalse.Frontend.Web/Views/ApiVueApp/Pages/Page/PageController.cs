using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

public class PageController(
    SessionUser _sessionUser,
    PageViewRepo _pageViewRepo,
    PermissionCheck _permissionCheck,
    KnowledgeSummaryLoader _knowledgeSummaryLoader,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    IHttpContextAccessor _httpContextAccessor,
    QuestionReadingRepo _questionReadingRepo)
    : Controller

{
    [HttpGet]
    public PageDataResult GetPage([FromRoute] int id, [CanBeNull] string shareToken)
    {
        var userAgent = Request.Headers["User-Agent"].ToString();

        if (!Settings.TrackersToIgnore.Any(item => userAgent.Contains(item)))
            _pageViewRepo.AddView(userAgent, id, _sessionUser.UserId);

        if (shareToken != null)
        {
            _sessionUser.AddShareToken(id, shareToken);
            _permissionCheck.OverWriteShareTokens(_sessionUser.ShareTokens);
        }

        var data = new PageDataManager(
                _sessionUser,
                _permissionCheck,
                _knowledgeSummaryLoader,
                _imageMetaDataReadingRepo,
                _httpContextAccessor,
                _questionReadingRepo)
            .GetPageData(id, shareToken);

        var canView = shareToken != null ? _permissionCheck.CanViewPage(data.Id, shareToken) : _permissionCheck.CanViewPage(data.Id);

        return new PageDataResult
        {
            Name = data.Name,
            Id = data.Id,
            Authors = data.Authors,
            AuthorIds = data.AuthorIds,
            CanAccess = data.CanAccess,
            CanBeDeleted = data.CanBeDeleted,
            ChildPageCount = data.ChildPageCount,
            Content = data.Content,
            CurrentUserIsCreator = data.CurrentUserIsCreator,
            DirectQuestionCount = data.DirectQuestionCount,
            DirectVisibleChildPageCount = data.DirectVisibleChildPageCount,
            GridItems = data.GridItems,
            ImageId = data.ImageId,
            ImageUrl = data.ImageUrl,
            IsChildOfPersonalWiki = data.IsChildOfPersonalWiki,
            IsWiki = data.IsWiki,
            KnowledgeSummary = data.KnowledgeSummary,
            MetaDescription = data.MetaDescription,
            ParentPageCount = data.ParentPageCount,
            Parents = data.Parents,
            QuestionCount = data.QuestionCount,
            PageItem = data.PageItem,
            Views = data.Views,
            Visibility = data.Visibility,
            TextIsHidden = data.TextIsHidden,
            MessageKey = data.MessageKey,
            ErrorCode = data.ErrorCode,
            Language = data.Language,
            CanEdit = _permissionCheck.CanEditPage(data.Id, shareToken),
            IsShared = canView && SharesService.IsShared(data.Id),
            SharedWith = canView ? GetSharedWithResponse(data.Id) : null,
        };
    }

    private List<SharedWithResponse> GetSharedWithResponse(int pageId)
    {
        return EntityCache.GetPageShares(pageId)
            .Where(share => share.SharedWith != null)
            .Select(share => new SharedWithResponse(
                share.SharedWith.Id,
                share.SharedWith.Name,
                new UserImageSettings(share.SharedWith.Id, _httpContextAccessor).GetUrl_20px_square(share.SharedWith).Url)
            )
            .ToList();
    }

    public readonly record struct SharedWithResponse(int Id, string Name, string ImgUrl);

    public record struct PageDataResult(
        bool CanAccess,
        int Id,
        string Name,
        string ImageUrl,
        string Content,
        int ParentPageCount,
        PageDataManager.Parent[] Parents,
        int ChildPageCount,
        int DirectVisibleChildPageCount,
        int Views,
        PageVisibility Visibility,
        int[] AuthorIds,
        PageDataManager.Author[] Authors,
        bool IsWiki,
        bool CurrentUserIsCreator,
        bool CanBeDeleted,
        int QuestionCount,
        int DirectQuestionCount,
        int ImageId,
        SearchPageItem PageItem,
        string MetaDescription,
        PageDataManager.KnowledgeSummarySlim KnowledgeSummary,
        PageGridManager.GridPageItem[] GridItems,
        bool IsChildOfPersonalWiki,
        bool TextIsHidden,
        string? MessageKey,
        NuxtErrorPageType? ErrorCode,
        List<DailyViews> ViewsLast30DaysAggregatedPage,
        List<DailyViews> ViewsLast30DaysPage,
        List<DailyViews> ViewsLast30DaysAggregatedQuestions,
        List<DailyViews> ViewsLast30DaysQuestions,
        string Language,
        bool CanEdit,
        bool IsShared,
        [CanBeNull] List<SharedWithResponse> SharedWith = null
    );
}