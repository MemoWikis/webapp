using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

public class DeleteTopicStoreController(
    SessionUser sessionUser,
    CategoryDeleter categoryDeleter,
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
        CategoryVisibility Visibility);

    private SuggestedNewParent FillSuggestedNewParent(CategoryCacheItem topic)
    {
        return new SuggestedNewParent
        {
            Id = topic.Id,
            Name = topic.Name,
            QuestionCount = topic.GetCountQuestionsAggregated(_sessionUser.UserId),
            ImageUrl = new CategoryImageSettings(topic.Id, _httpContextAccessor)
                .GetUrl_128px(asSquare: true).Url,
            MiniImageUrl = new ImageFrontendData(
                    _imageMetaDataReadingRepo.GetBy(topic.Id, ImageType.Category),
                    _httpContextAccessor,
                    _questionReadingRepo)
                .GetImageUrl(30, true, false, ImageType.Category)
                .Url,
            Visibility = topic.Visibility
        };
    }

    public readonly record struct DeleteData(
        string Name,
        bool HasChildren,
        SuggestedNewParent? SuggestedNewParent,
        bool hasPublicQuestion);

    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public DeleteData GetDeleteData([FromRoute] int id)
    {
        var topic = EntityCache.GetCategory(id);
        var children = GraphService.Descendants(id);
        var hasChildren = children.Count > 0;
        if (topic == null)
            throw new Exception(
                "Category couldn't be deleted. Category with specified Id cannot be found.");

        var currentWiki = EntityCache.GetCategory(_sessionUser.CurrentWikiId);

        var hasPublicQuestion = EntityCache
            .GetCategory(id)
            .GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId, false)
            .Any(q => q.Visibility == QuestionVisibility.All);

        var parents = _crumbtrailService.BuildCrumbtrail(topic, currentWiki);
        var newParentId =
            new SearchHelper(_imageMetaDataReadingRepo, _httpContextAccessor, _questionReadingRepo)
                .SuggestNewParent(parents, hasPublicQuestion);

        if (newParentId == null)
            return new DeleteData(topic.Name, hasChildren, null, hasPublicQuestion);

        var suggestedNewParent = FillSuggestedNewParent(EntityCache.GetCategory((int)newParentId));

        return new DeleteData(topic.Name, hasChildren, suggestedNewParent, hasPublicQuestion);
    }

    public readonly record struct DeleteJson(int id, int parentForQuestionsId);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public CategoryDeleter.DeleteTopicResult Delete([FromBody] DeleteJson deleteJson)
    {
        if (deleteJson.parentForQuestionsId == 0)
            return new CategoryDeleter.DeleteTopicResult(Success: false,
                MessageKey: FrontendMessageKeys.Error.Category.TopicNotSelected);

        if (deleteJson.parentForQuestionsId == deleteJson.id)
            return new CategoryDeleter.DeleteTopicResult(Success: false,
                MessageKey: FrontendMessageKeys.Error.Category.NewTopicIdIsTopicIdToBeDeleted);

        return categoryDeleter.DeleteTopic(deleteJson.id, deleteJson.parentForQuestionsId);
    }
}