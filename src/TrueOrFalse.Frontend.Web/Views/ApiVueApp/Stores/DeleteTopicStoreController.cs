using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using static PageDeleter;

public class DeletePageStoreController(
    SessionUser sessionUser,
    PageDeleter pageDeleter,
    CrumbtrailService _crumbtrailService,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    IHttpContextAccessor _httpContextAccessor,
    QuestionReadingRepo _questionReadingRepo) : BaseController(sessionUser)
{
    public record struct SuggestedNewParent(
        int Id,
        string Name,
        int QuestionCount,
        string ImageUrl,
        string MiniImageUrl,
        PageVisibility Visibility);

    private SuggestedNewParent FillSuggestedNewParent(PageCacheItem topic)
    {
        return new SuggestedNewParent
        {
            Id = topic.Id,
            Name = topic.Name,
            QuestionCount = topic.GetCountQuestionsAggregated(_sessionUser.UserId),
            ImageUrl = new PageImageSettings(topic.Id, _httpContextAccessor)
                .GetUrl_128px(asSquare: true).Url,
            MiniImageUrl = new ImageFrontendData(
                    _imageMetaDataReadingRepo.GetBy(topic.Id, ImageType.Page),
                    _httpContextAccessor,
                    _questionReadingRepo)
                .GetImageUrl(30, true, false, ImageType.Page)
                .Url,
            Visibility = topic.Visibility
        };
    }

    public readonly record struct DeleteData(
        string Name,
        bool HasChildren,
        SuggestedNewParent? SuggestedNewParent,
        bool HasQuestion,
        bool HasPublicQuestion);

    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public DeleteData GetDeleteData([FromRoute] int id)
    {
        var topic = EntityCache.GetPage(id);
        var children = GraphService.Descendants(id);
        var hasChildren = children.Count > 0;
        if (topic == null)
            throw new Exception(
                "Category couldn't be deleted. Category with specified Id cannot be found.");

        var questions = EntityCache
            .GetPage(id)?
            .GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId, false);

        var hasQuestion = questions?.Count > 0;

        if (!hasChildren && !hasQuestion)
            return new DeleteData(topic.Name, HasChildren: false, SuggestedNewParent: null, HasQuestion: false, HasPublicQuestion: false);

        var hasPublicQuestion = questions?
            .Any(q => q.Visibility == QuestionVisibility.All) ?? false;

        var currentWiki = EntityCache.GetPage(_sessionUser.CurrentWikiId);

        var parents = _crumbtrailService.BuildCrumbtrail(topic, currentWiki);

        var newParentId =
            new SearchHelper(_imageMetaDataReadingRepo, _httpContextAccessor, _questionReadingRepo)
                .SuggestNewParent(parents, hasPublicQuestion);

        if (newParentId == null)
            return new DeleteData(topic.Name, hasChildren, SuggestedNewParent: null, hasQuestion, hasPublicQuestion);

        var suggestedNewParent = FillSuggestedNewParent(EntityCache.GetPage((int)newParentId));

        return new DeleteData(topic.Name, hasChildren, suggestedNewParent, hasQuestion, hasPublicQuestion);
    }

    public readonly record struct DeleteRequest(int PageToDeleteId, int? ParentForQuestionsId);

    public record struct DeleteResponse(
        bool Success,
        bool? HasChildren = null,
        bool? IsNotCreatorOrAdmin = null,
        RedirectParent? RedirectParent = null,
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
                return new DeleteResponse(Success: false, MessageKey: FrontendMessageKeys.Error.Page.NewPageIdIsPageIdToBeDeleted);
        }

        var deleteResult = pageDeleter.DeletePage(deleteRequest.PageToDeleteId, deleteRequest.ParentForQuestionsId);

        return new DeleteResponse(
            Success: deleteResult.Success,
            HasChildren: deleteResult.HasChildren,
            IsNotCreatorOrAdmin: deleteResult.IsNotCreatorOrAdmin,
            RedirectParent: deleteResult.RedirectParent);
    }
}