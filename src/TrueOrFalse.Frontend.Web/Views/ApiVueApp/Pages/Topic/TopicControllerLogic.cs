
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;

public class TopicControllerLogic(SessionUser sessionUser,
    PermissionCheck permissionCheck,
    GridItemLogic gridItemLogic,
    KnowledgeSummaryLoader knowledgeSummaryLoader,
    CategoryViewRepo categoryViewRepo,
    ImageMetaDataReadingRepo imageMetaDataReadingRepo,
    IHttpContextAccessor httpContextAccessor,
    IWebHostEnvironment webHostEnvironment,
    IActionContextAccessor actionContextAccessor,
    QuestionReadingRepo questionReadingRepo) : IRegisterAsInstancePerLifetime
{

    private dynamic CreateTopicDataObject(int id, CategoryCacheItem topic, ImageMetaData imageMetaData, KnowledgeSummary knowledgeSummary)
    {
        
        return new
        {
            CanAccess = true,
            Id = id,
            Name = topic.Name,
                ImageUrl = new CategoryImageSettings(id, httpContextAccessor, webHostEnvironment).GetUrl_128px(asSquare: true).Url,
            Content = topic.Content,
            ParentTopicCount = topic.ParentCategories().Where(permissionCheck.CanView).ToList().Count,
            Parents = topic.ParentCategories().Where(permissionCheck.CanView).Select(p => 
                new { id = p.Id, name = p.Name, imgUrl = new CategoryImageSettings(p.Id, httpContextAccessor, webHostEnvironment)
                    .GetUrl(50, true)
                    .Url })
                .ToArray(),

            ChildTopicCount = topic.AggregatedCategories(permissionCheck, false).Count,
            DirectChildTopicCount = topic.DirectChildrenIds.Where(permissionCheck.CanViewCategory).ToList().Count,
                Views = categoryViewRepo.GetViewCount(id),
            Visibility = topic.Visibility,
            AuthorIds = topic.AuthorIds,
            Authors = topic.AuthorIds.Select(id =>
            {
                var author = EntityCache.GetUserById(id);
                return new
                {
                    Id = id,
                    Name = author.Name,
                        ImgUrl = new UserImageSettings(author.Id, httpContextAccessor, webHostEnvironment).GetUrl_20px(author).Url,
                    Reputation = author.Reputation,
                    ReputationPos = author.ReputationPos
                };
            }).ToArray(),
            IsWiki = topic.IsStartPage(),
            CurrentUserIsCreator = sessionUser.User != null && sessionUser.UserId == topic.Creator?.Id,
            CanBeDeleted = sessionUser.User != null && permissionCheck.CanDelete(topic),
            QuestionCount = topic.GetAggregatedQuestionsFromMemoryCache(sessionUser.UserId).Count,
            DirectQuestionCount = topic.GetCountQuestionsAggregated(sessionUser.UserId, true),
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
            gridItems = gridItemLogic.GetChildren(id),
            isChildOfPersonalWiki = sessionUser.IsLoggedIn && EntityCache.GetCategory(sessionUser.User.StartTopicId).DirectChildrenIds.Any(id => id == topic.Id)
        };
    }


    public dynamic GetTopicData(int id)
    {
        var topic = EntityCache.GetCategory(id);

        if (permissionCheck.CanView(sessionUser.UserId, topic))
        {
            var imageMetaData = imageMetaDataReadingRepo.GetBy(id, ImageType.Category);
            var knowledgeSummary = knowledgeSummaryLoader.RunFromMemoryCache(id, sessionUser.UserId);

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
            Url = new Links(actionContextAccessor, httpContextAccessor)
                .CategoryDetail(topic.Name, topic.Id),
            QuestionCount = topic.GetCountQuestionsAggregated(sessionUser.UserId),
            ImageUrl = new CategoryImageSettings(topic.Id,
                    httpContextAccessor, 
                    webHostEnvironment)
                .GetUrl_128px(asSquare: true)
                .Url,
            MiniImageUrl = new ImageFrontendData(imageMetaDataReadingRepo.GetBy(topic.Id, ImageType.Category),
                    httpContextAccessor, 
                    webHostEnvironment,
                    questionReadingRepo)
                .GetImageUrl(30, true, false, ImageType.Category).Url,
            Visibility = (int)topic.Visibility
        };

        return miniTopicItem;
    }
}

