using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

public class PageController(
    SessionUser _sessionUser,
    PageViewRepo pageViewRepo,
    PermissionCheck _permissionCheck,
    KnowledgeSummaryLoader _knowledgeSummaryLoader,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    IHttpContextAccessor _httpContextAccessor,
    QuestionReadingRepo _questionReadingRepo)
    : Controller

{
    [HttpGet]
    public PageDataResult GetPage([FromRoute] int id)
    {
        var userAgent = Request.Headers["User-Agent"].ToString();

        if (!Settings.TrackersToIgnore.Any(item => userAgent.Contains(item)))
            pageViewRepo.AddView(userAgent, id, _sessionUser.UserId);

        var data = new PageDataManager(
                _sessionUser,
                _permissionCheck,
                _knowledgeSummaryLoader,
                _imageMetaDataReadingRepo,
                _httpContextAccessor,
                _questionReadingRepo)
            .GetPageData(id);

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
            Language = data.Language
        };
    }

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
        string Language
    );
}