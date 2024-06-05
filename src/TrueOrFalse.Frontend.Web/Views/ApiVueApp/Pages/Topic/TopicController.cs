using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class TopicController(
    SessionUser _sessionUser,
    CategoryViewRepo _categoryViewRepo,
    PermissionCheck _permissionCheck,
    KnowledgeSummaryLoader _knowledgeSummaryLoader,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    IHttpContextAccessor _httpContextAccessor,
    QuestionReadingRepo _questionReadingRepo)
    : Controller
{
    [HttpGet]
    public TopicDataResult GetTopic([FromRoute] int id)
    {
        var userAgent = Request.Headers["User-Agent"].ToString();
        _categoryViewRepo.AddView(userAgent, id, _sessionUser.UserId);

        var data = new TopicDataManager(
                _sessionUser,
                _permissionCheck,
                _knowledgeSummaryLoader,
                _categoryViewRepo,
                _imageMetaDataReadingRepo,
                _httpContextAccessor,
                _questionReadingRepo)
            .GetTopicData(id);

        if (data == null)
            return new TopicDataResult();

        return new TopicDataResult
        {
            Name = data.Name,
            Id = data.Id,
            Authors = data.Authors,
            AuthorIds = data.AuthorIds,
            CanAccess = data.CanAccess,
            CanBeDeleted = data.CanBeDeleted,
            ChildTopicCount = data.ChildTopicCount,
            Content = data.Content,
            CurrentUserIsCreator = data.CurrentUserIsCreator,
            DirectQuestionCount = data.DirectQuestionCount,
            DirectVisibleChildTopicCount = data.DirectVisibleChildTopicCount,
            GridItems = data.GridItems,
            ImageId = data.ImageId,
            ImageUrl = data.ImageUrl,
            IsChildOfPersonalWiki = data.IsChildOfPersonalWiki,
            IsWiki = data.IsWiki,
            KnowledgeSummary = data.KnowledgeSummary,
            MetaDescription = data.MetaDescription,
            ParentTopicCount = data.ParentTopicCount,
            Parents = data.Parents,
            QuestionCount = data.QuestionCount,
            TopicItem = data.TopicItem,
            Views = data.Views,
            Visibility = data.Visibility,
            IsHideText = data.IsHideText
        };
    }

    public record struct TopicDataResult(
        bool CanAccess,
        int Id,
        string Name,
        string ImageUrl,
        string Content,
        int ParentTopicCount,
        TopicDataManager.Parent[] Parents,
        int ChildTopicCount,
        int DirectVisibleChildTopicCount,
        int Views,
        CategoryVisibility Visibility,
        int[] AuthorIds,
        TopicDataManager.Author[] Authors,
        bool IsWiki,
        bool CurrentUserIsCreator,
        bool CanBeDeleted,
        int QuestionCount,
        int DirectQuestionCount,
        int ImageId,
        SearchTopicItem TopicItem,
        string MetaDescription,
        TopicDataManager.KnowledgeSummarySlim KnowledgeSummary,
        TopicGridManager.GridTopicItem[] GridItems,
        bool IsChildOfPersonalWiki,
        bool IsHideText);
}