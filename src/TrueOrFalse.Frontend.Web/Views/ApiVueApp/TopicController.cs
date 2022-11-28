using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;

public class TopicController : BaseController
{
    [HttpGet]
    public JsonResult GetTopic(int id)
    {
        var category = EntityCache.GetCategory(id);

        if (PermissionCheck.CanView(category))
            return Json( new
            {
                CanAccess = true,
                Id = id,
                Name = category.Name,
                ImgUrl = new CategoryImageSettings(id).GetUrl_128px(asSquare: true).Url,
                Content = category.Content,
                ParentTopicCount = category.ParentCategories().Count,
                ChildTopicCount = category.AggregatedCategories().Count,
                Views = Sl.CategoryViewRepo.GetViewCount(id),
                Visibility = category.Visibility,
                AuthorIds = category.AuthorIds,
                Authors = category.AuthorIds.Select(id =>
                {
                    var author = UserCache.GetUser(id);
                    return new
                    {
                        Id = id,
                        Name = author.Name,
                        ImgUrl = new UserImageSettings(author.Id).GetUrl_20px(author).Url,
                        Reputation = author.Reputation,
                        ReputationPos = author.ReputationPos
                    };

                }).ToArray(),
                IsWiki = category.IsStartPage(),
                CurrentUserIsCreator = SessionUser.User != null && SessionUser.User.Id == category.Creator.Id,
                CanBeDeleted = SessionUser.User != null && PermissionCheck.CanDelete(category),
                QuestionCount = category.CountQuestionsAggregated
            }, JsonRequestBehavior.AllowGet);
        else
        {
            var json = Json(new TopicModel(), JsonRequestBehavior.AllowGet);
            return json;
        }
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

    private List<AuthorItem> GetAuthorItems(int[] ids)
    {
        var authorItems = new List<AuthorItem>();
        foreach (int id in ids)
        {
            var author = UserCache.GetUser(id);
            authorItems.Add(new AuthorItem
            {
                Name = author.Name,
                Id = id,
                ImageURL = new UserImageSettings(author.Id).GetUrl_20px(author).Url,
                Reputation = author.Reputation,
                ReputationPos = author.ReputationPos
            });
        }

        return authorItems;
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
}
public class AuthorItem
{
    public string Name;
    public string ImageURL;
    public int Id;
    public int Reputation;
    public int ReputationPos;
}

public class TopicModel
{
    public bool CanAccess { get; set; } = false;
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImgUrl { get; set; }
    public string Content { get; set; }
    public int ParentTopicCount { get; set; }
    public int ChildTopicCount { get; set; }
    public int Views { get; set; }
    public CategoryVisibility Visibility { get; set; }

    //Comments not implemented yet
    public int CommentCount { get; set; }
    public int[] AuthorIds { get; set; }
    public bool IsWiki { get; set; }
    public bool CurrentUserIsCreator { get; set; }
    public bool CanBeDeleted { get; set; }
    public int QuestionCount { get; set; }
}