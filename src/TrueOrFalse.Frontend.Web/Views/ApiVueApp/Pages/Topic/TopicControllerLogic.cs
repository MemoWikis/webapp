
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

namespace VueApp;

public class TopicControllerLogic 
{
    public string SaveTopic(int id, string name, bool saveName, string content, bool saveContent)
    {
        if (!PermissionCheck.CanEditCategory(id))
            return JsonConvert.SerializeObject("Dir fehlen leider die Rechte um die Seite zu bearbeiten");

        if(!PermissionCheck.CanSave())
            return JsonConvert.SerializeObject("Möglicherweise sollten Sie einige private Themen öffentlich machen" +
                                               " und ein Abonnement in Betracht ziehen, um mehr Funktionen zu erhalten.");

        var categoryCacheItem = EntityCache.GetCategory(id);
        var category = Sl.CategoryRepo.GetById(categoryCacheItem.Id);

        if (categoryCacheItem == null || category == null)
            return JsonConvert.SerializeObject(false);

        if (saveName)
        {
            categoryCacheItem.Name = name;
            category.Name = name;
        }

        if (saveContent)
        {
            categoryCacheItem.Content = content;
            category.Content = content;
        }
        EntityCache.AddOrUpdate(categoryCacheItem);
        Sl.CategoryRepo.Update(category, SessionUser.User, type: CategoryChangeType.Text);

        return JsonConvert.SerializeObject(true);
    }

    public dynamic GetTopicData(int id)
    {
        var topic = EntityCache.GetCategory(id);

        if (PermissionCheck.CanView(topic))
        {
            var imageMetaData = Sl.ImageMetaDataRepo.GetBy(id, ImageType.Category);
            var knowledgeSummary = KnowledgeSummaryLoader.RunFromMemoryCache(id, SessionUser.UserId);

            return new
            {
                CanAccess = true,
                Id = id,
                Name = topic.Name,
                ImageUrl = new CategoryImageSettings(id).GetUrl_128px(asSquare: true).Url,
                Content = topic.Content,
                ParentTopicCount = topic.ParentCategories().Count,
                ChildTopicCount = topic.AggregatedCategories().Count,
                Views = Sl.CategoryViewRepo.GetViewCount(id),
                Visibility = topic.Visibility,
                AuthorIds = topic.AuthorIds,
                Authors = topic.AuthorIds.Select(id =>
                {
                    var author = EntityCache.GetUserById(id);
                    return new
                    {
                        Id = id,
                        Name = author.Name,
                        ImgUrl = new UserImageSettings(author.Id).GetUrl_20px(author).Url,
                        Reputation = author.Reputation,
                        ReputationPos = author.ReputationPos
                    };
                }).ToArray(),
                IsWiki = topic.IsStartPage(),
                CurrentUserIsCreator = SessionUser.User != null && SessionUser.UserId == topic.Creator?.Id,
                CanBeDeleted = SessionUser.User != null && PermissionCheck.CanDelete(topic),
                QuestionCount = topic.GetAggregatedQuestionsFromMemoryCache().Count,
                ImageId = imageMetaData != null ? imageMetaData.Id : 0,
                EncodedName = UriSanitizer.Run(topic.Name),
                SearchTopicItem = FillMiniTopicItem(topic),
                MetaDescription = SeoUtils.ReplaceDoubleQuotes(topic.Content == null ? null : Regex.Replace(topic.Content, "<.*?>", "")).Truncate(250, true),
                KnowledgeSummary = new
                {
                    notLearned = knowledgeSummary.NotLearned + knowledgeSummary.NotInWishknowledge,
                    needsLearning = knowledgeSummary.NeedsLearning,
                    needsConsolidation = knowledgeSummary.NeedsConsolidation,
                    solid = knowledgeSummary.Solid
                }
            };
        }

        return new { };
    }

    private SearchTopicItem FillMiniTopicItem(CategoryCacheItem topic)
    {
        var miniTopicItem = new SearchTopicItem
        {
            Id = topic.Id,
            Name = topic.Name,
            Url = Links.CategoryDetail(topic.Name, topic.Id),
            QuestionCount = topic.GetCountQuestionsAggregated(),
            ImageUrl = new CategoryImageSettings(topic.Id).GetUrl_128px(asSquare: true).Url,
            MiniImageUrl = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(topic.Id, ImageType.Category))
                .GetImageUrl(30, true, false, ImageType.Category).Url,
            Visibility = (int)topic.Visibility
        };

        return miniTopicItem;
    }

    public SearchTopicItem FillBasicTopicItem(CategoryCacheItem topic)
    {
        var isVisible = PermissionCheck.CanView(topic);
        var basicTopicItem = new SearchTopicItem
        {
            Id = topic.Id,
            Name = topic.Name,
            Url = Links.CategoryDetail(topic.Name, topic.Id),
            QuestionCount = topic.GetCountQuestionsAggregated(),
            ImageUrl = new CategoryImageSettings(topic.Id).GetUrl_128px(asSquare: true).Url,
            MiniImageUrl = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(topic.Id, ImageType.Category))
                .GetImageUrl(30, true, false, ImageType.Category).Url,
            Visibility = (int)topic.Visibility
        };

        return basicTopicItem;
    }
}

