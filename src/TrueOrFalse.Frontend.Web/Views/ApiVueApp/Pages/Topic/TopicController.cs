using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

namespace VueApp;

public class TopicController : BaseController
{
    [HttpGet]
    public JsonResult GetTopic(int id)
    {
        return Json(GetTopicData(id), JsonRequestBehavior.AllowGet);
    }

    public dynamic GetTopicData(int id)
    {
        var topic = EntityCache.GetCategory(id);

        if (PermissionCheck.CanView(topic))
        {
            var imageMetaData = Sl.ImageMetaDataRepo.GetBy(id, ImageType.Category);
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
                QuestionCount = topic.CountQuestionsAggregated,
                ImageId = imageMetaData != null ? imageMetaData.Id : 0,
                EncodedName = UriSanitizer.Run(topic.Name)
            };
        }

        return new { };
    }

    [HttpGet]
    public bool CanAccess(int id)
    {
        var c = EntityCache.GetCategory(id);

        if (PermissionCheck.CanView(c))
            return true;

        return false;
    }

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public JsonResult SaveTopic(int id, string name, bool saveName, string content, bool saveContent)
    {
        if (!PermissionCheck.CanEditCategory(id))
            return Json("Dir fehlen leider die Rechte um die Seite zu bearbeiten");

        var categoryCacheItem = EntityCache.GetCategory(id);
        var category = Sl.CategoryRepo.GetById(categoryCacheItem.Id);

        if (categoryCacheItem == null || category == null)
            return Json(false);

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

        return Json(true);
    }

    [HttpPost]
    public JsonResult GetBasicTopicItem(int categoryId)
    {
        var category = EntityCache.GetCategory(categoryId);

        var json = new JsonResult
        {
            Data = new
            {
                Topic = FillBasicTopicItem(category)
            }
        };

        return json;
    }

    public SearchTopicItem FillBasicTopicItem(Category topic)
    {
        var cacheItem = EntityCache.GetCategory(topic.Id);

        return FillBasicTopicItem(cacheItem);
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
            IconHtml = SearchApiController.GetIconHtml(topic),
            MiniImageUrl = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(topic.Id, ImageType.Category))
                .GetImageUrl(30, true, false, ImageType.Category).Url,
            Visibility = (int)topic.Visibility
        };

        return basicTopicItem;
    }


    [HttpGet]
    public JsonResult LoadQuestionIds(int topicId)
    {
        var topicCacheItem = EntityCache.GetCategory(topicId);
        if (PermissionCheck.CanView(topicCacheItem))
        {
            var userCacheItem = SessionUserCache.GetItem(User_().Id);
            return Json(topicCacheItem
                .GetAggregatedQuestionsFromMemoryCache()
                .Where(q =>
                    q.Creator.Id == userCacheItem.Id &&
                    q.IsPrivate() &&
                    PermissionCheck.CanEdit(q))
                .Select(q => q.Id).ToList(), JsonRequestBehavior.AllowGet);
        }

        return Json(new { }, JsonRequestBehavior.DenyGet);
    }
}

