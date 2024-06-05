using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

public class TopicDataManager(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    KnowledgeSummaryLoader _knowledgeSummaryLoader,
    CategoryViewRepo _categoryViewRepo,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    IHttpContextAccessor _httpContextAccessor,
    QuestionReadingRepo _questionReadingRepo)
{
    public TopicDataResult GetTopicData(int id)
    {
        var topic = EntityCache.GetCategory(id);

        if (_permissionCheck.CanView(_sessionUser.UserId, topic))
        {
            var imageMetaData = _imageMetaDataReadingRepo.GetBy(id, ImageType.Category);
            var knowledgeSummary =
                _knowledgeSummaryLoader.RunFromMemoryCache(id, _sessionUser.UserId);

            return CreateTopicDataObject(id, topic, imageMetaData, knowledgeSummary);
        }

        return null;
    }

    private SearchTopicItem FillMiniTopicItem(CategoryCacheItem topic)
    {
        var miniTopicItem = new SearchTopicItem
        {
            Id = topic.Id,
            Name = topic.Name,
            QuestionCount = topic.GetCountQuestionsAggregated(_sessionUser.UserId),
            ImageUrl = new CategoryImageSettings(topic.Id,
                    _httpContextAccessor)
                .GetUrl_128px(true)
                .Url,
            MiniImageUrl = new ImageFrontendData(
                    _imageMetaDataReadingRepo.GetBy(topic.Id, ImageType.Category),
                    _httpContextAccessor,
                    _questionReadingRepo)
                .GetImageUrl(30, true, false, ImageType.Category).Url,
            Visibility = (int)topic.Visibility
        };

        return miniTopicItem;
    }

    private TopicDataResult CreateTopicDataObject(
        int id,
        CategoryCacheItem topic,
        ImageMetaData imageMetaData,
        KnowledgeSummary knowledgeSummary)
    {
        var authorIds = topic.AuthorIds.Distinct();
        return new TopicDataResult(
            CanAccess: true,
            Id: id,
            Name: topic.Name,
            ImageUrl: new CategoryImageSettings(id, _httpContextAccessor).GetUrl_128px(true).Url,
            Content: topic.Content,
            ParentTopicCount: topic.Parents()
                .Where(_permissionCheck.CanView)
                .ToList()
                .Count,
            Parents: topic.Parents()
                .Where(_permissionCheck.CanView)
                .Select(p =>
                    new Parent
                    {
                        Id = p.Id,
                        Name = p.Name,
                        ImgUrl = new CategoryImageSettings(p.Id, _httpContextAccessor)
                            .GetUrl(50, true)
                            .Url
                    })
                .ToArray(),
            ChildTopicCount: GraphService
                .VisibleDescendants(topic.Id, _permissionCheck, _sessionUser.UserId).Count,
            DirectVisibleChildTopicCount: GraphService
                .VisibleChildren(topic.Id, _permissionCheck, _sessionUser.UserId).Count,
            Views: _categoryViewRepo.GetViewCount(id),
            Visibility: topic.Visibility,
            AuthorIds: authorIds.ToArray(),
            Authors: authorIds.Select(authorId =>
            {
                var author = EntityCache.GetUserById(authorId);
                return new Author(
                    authorId,
                    author.Name,
                    new UserImageSettings(author.Id, _httpContextAccessor)
                        .GetUrl_20px_square(author).Url,
                    author.Reputation,
                    author.ReputationPos
                );
            }).ToArray(),
            IsWiki: topic.IsStartPage(),
            CurrentUserIsCreator: CurrentUserIsCreator(topic),
            CanBeDeleted: _permissionCheck.CanDelete(topic),
            QuestionCount: topic.GetCountQuestionsAggregated(_sessionUser.UserId),
            DirectQuestionCount: topic.GetCountQuestionsAggregated(_sessionUser.UserId, true,
                topic.Id),
            ImageId: imageMetaData != null ? imageMetaData.Id : 0,
            TopicItem: FillMiniTopicItem(topic),
            MetaDescription: SeoUtils.ReplaceDoubleQuotes(topic.Content == null
                    ? null
                    : Regex.Replace(topic.Content, "<.*?>", ""))
                .Truncate(250, true),
            KnowledgeSummary: new KnowledgeSummarySlim(
                NotLearned: knowledgeSummary.NotLearned + knowledgeSummary.NotInWishknowledge,
                NeedsLearning: knowledgeSummary.NeedsLearning,
                NeedsConsolidation: knowledgeSummary.NeedsConsolidation,
                Solid: knowledgeSummary.Solid
            ),
            GridItems: new TopicGridManager(
                _permissionCheck,
                _sessionUser,
                _imageMetaDataReadingRepo,
                _httpContextAccessor,
                _knowledgeSummaryLoader,
                _questionReadingRepo).GetChildren(id),
            IsChildOfPersonalWiki: _sessionUser.IsLoggedIn && EntityCache
                .GetCategory(_sessionUser.User.StartTopicId)
                .ChildRelations
                .Any(r => r.ChildId == topic.Id),
            IsHideText: topic.IsHideText
        );
    }

    private bool CurrentUserIsCreator(CategoryCacheItem topic)
    {
        if (_sessionUser.IsLoggedIn == false)
            return false;

        return _sessionUser.UserId == topic.Creator?.Id;
    }

    public record Author(int Id, string Name, string ImgUrl, int Reputation, int ReputationPos);

    public class Parent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
    }

    public record struct KnowledgeSummarySlim(
        int NotLearned,
        int NeedsLearning,
        int NeedsConsolidation,
        int Solid);

    public record TopicDataResult(
        bool CanAccess,
        int Id,
        string Name,
        string ImageUrl,
        string Content,
        int ParentTopicCount,
        Parent[] Parents,
        int ChildTopicCount,
        int DirectVisibleChildTopicCount,
        int Views,
        CategoryVisibility Visibility,
        int[] AuthorIds,
        Author[] Authors,
        bool IsWiki,
        bool CurrentUserIsCreator,
        bool CanBeDeleted,
        int QuestionCount,
        int DirectQuestionCount,
        int ImageId,
        SearchTopicItem TopicItem,
        string MetaDescription,
        KnowledgeSummarySlim KnowledgeSummary,
        TopicGridManager.GridTopicItem[] GridItems,
        bool IsChildOfPersonalWiki,
        bool IsHideText
    );
}