using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace VueApp;

public class TopicControllerLogic : IRegisterAsInstancePerLifetime
{
    private readonly SessionUser _sessionUser;
    private readonly PermissionCheck _permissionCheck;
    private readonly GridItemLogic _gridItemLogic;
    private readonly KnowledgeSummaryLoader _knowledgeSummaryLoader;
    private readonly CategoryViewRepo _categoryViewRepo;
    private readonly ImageMetaDataReadingRepo _imageMetaDataReadingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly QuestionReadingRepo _questionReadingRepo;

    public TopicControllerLogic(SessionUser sessionUser,
        PermissionCheck permissionCheck,
        GridItemLogic gridItemLogic,
        KnowledgeSummaryLoader knowledgeSummaryLoader,
        CategoryViewRepo categoryViewRepo,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        IHttpContextAccessor httpContextAccessor,
        QuestionReadingRepo questionReadingRepo)
    {
        _sessionUser = sessionUser;
        _permissionCheck = permissionCheck;
        _gridItemLogic = gridItemLogic;
        _knowledgeSummaryLoader = knowledgeSummaryLoader;
        _categoryViewRepo = categoryViewRepo;
        _imageMetaDataReadingRepo = imageMetaDataReadingRepo;
        _httpContextAccessor = httpContextAccessor;
        _questionReadingRepo = questionReadingRepo;
    }
    public dynamic GetTopicData(int id)
    {
        var topic = EntityCache.GetCategory(id);

        if (_permissionCheck.CanView(_sessionUser.UserId, topic))
        {
            var imageMetaData = _imageMetaDataReadingRepo.GetBy(id, ImageType.Category);
            var knowledgeSummary = _knowledgeSummaryLoader.RunFromMemoryCache(id, _sessionUser.UserId);

            return CreateTopicDataObject(id, topic, imageMetaData, knowledgeSummary);
        }

        return new { };
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
                .GetUrl_128px(asSquare: true)
                .Url,
            MiniImageUrl = new ImageFrontendData(_imageMetaDataReadingRepo.GetBy(topic.Id, ImageType.Category),
                    _httpContextAccessor, 
                    _questionReadingRepo)
                .GetImageUrl(30, true, false, ImageType.Category).Url,
            Visibility = (int)topic.Visibility
        };

        return miniTopicItem;
    }

    private dynamic CreateTopicDataObject(int id, CategoryCacheItem topic, ImageMetaData imageMetaData, KnowledgeSummary knowledgeSummary)
    {
        return new
        {
            CanAccess = true,
            Id = id,
            Name = topic.Name,
            ImageUrl = new CategoryImageSettings(id, _httpContextAccessor).GetUrl_128px(asSquare: true).Url,
            Content = topic.Content,
            ParentTopicCount = topic.ParentCategories().Where(_permissionCheck.CanView).ToList().Count,
            Parents = topic.ParentCategories().Where(_permissionCheck.CanView).Select(p =>
                new {
                    id = p.Id,
                    name = p.Name,
                    imgUrl = new CategoryImageSettings(p.Id, _httpContextAccessor)
                    .GetUrl(50, true)
                    .Url
                })
                .ToArray(),

            ChildTopicCount = topic.AggregatedCategories(_permissionCheck, false).Count,
            DirectChildTopicCount = topic.DirectChildrenIds.Where(_permissionCheck.CanViewCategory).ToList().Count,
            Views = _categoryViewRepo.GetViewCount(id),
            Visibility = topic.Visibility,
            AuthorIds = topic.AuthorIds.Distinct().ToArray(),
            Authors = topic.AuthorIds.Distinct().Select(authorId =>
            {
                var author = EntityCache.GetUserById(authorId);
                return new
                {
                    Id = authorId,
                    Name = author.Name,
                    ImgUrl = new UserImageSettings(author.Id, _httpContextAccessor).GetUrl_20px_square(author).Url,
                    Reputation = author.Reputation,
                    ReputationPos = author.ReputationPos
                };
            }).ToArray(),
            IsWiki = topic.IsStartPage(),
            CurrentUserIsCreator = _sessionUser.User != null && _sessionUser.UserId == topic.Creator?.Id,
            CanBeDeleted = _sessionUser.User != null && _permissionCheck.CanDelete(topic),
            QuestionCount = topic.GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId).Count,
            DirectQuestionCount = topic.GetCountQuestionsAggregated(_sessionUser.UserId, true),
            ImageId = imageMetaData != null ? imageMetaData.Id : 0,
            SearchTopicItem = FillMiniTopicItem(topic),
            MetaDescription = SeoUtils.ReplaceDoubleQuotes(topic.Content == null ? null : Regex.Replace(topic.Content, "<.*?>", "")).Truncate(250, true),
            KnowledgeSummary = new
            {
                notLearned = knowledgeSummary.NotLearned + knowledgeSummary.NotInWishknowledge,
                needsLearning = knowledgeSummary.NeedsLearning,
                needsConsolidation = knowledgeSummary.NeedsConsolidation,
                solid = knowledgeSummary.Solid
            },
            gridItems = _gridItemLogic.GetChildren(id),
            isChildOfPersonalWiki = _sessionUser.IsLoggedIn && EntityCache.GetCategory(_sessionUser.User.StartTopicId).DirectChildrenIds.Any(id => id == topic.Id)
        };
    }
}

