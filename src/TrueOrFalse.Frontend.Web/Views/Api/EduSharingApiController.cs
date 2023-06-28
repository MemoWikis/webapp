﻿using System;
using System.Linq;
using System.Web.Mvc;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Frontend.Web.Code;

public class EduSharingApiController : BaseController
{

    public JsonResult Topic(int id)
    {
        var category = Sl.CategoryRepo.GetById(id);

        return Json(new
        {
            Title = category.Name,
            Keyword = category.ParentCategories().Select(x => x.Name).Aggregate((a, b) => a + ", " + b),
            DateCreated = new DateTimeOffset(category.DateCreated).ToUnixTimeSeconds(),
            DateModified = new DateTimeOffset(category.DateModified).ToUnixTimeSeconds(),
            Format = "",
            Creator = new UserTinyModel(EntityCache.GetUserById(category.Creator != null ? category.Creator.Id : -1)).Name,
            CreatorMetaData = new UserTinyModel(EntityCache.GetUserById(category.Creator != null ? category.Creator.Id : -1)).Name,
            Url = Settings.CanonicalHost + Links.CategoryDetail(category.Name, category.Id),
            Thumbnail = Settings.CanonicalHost + new CategoryImageSettings(category.Id).GetUrl_350px_square().Url
        }, JsonRequestBehavior.AllowGet);
    }


    //public JsonResult Search(string term, int pageSize = 5, int page = 1)
    //{
    //    var result = Sl.SearchCategories.Run(term, new Pager { PageSize = pageSize, IgnorePageCount = true, CurrentPage = page});
    //    var jsonResult = new JsonResult();
    //    jsonResult = Json(new
    //    {
    //        ResultCount = result.Count,
    //        Items = result
    //            .GetCategories()
    //            .Select(category => new
    //            {
    //                TopicId = category.Id,
    //                Name = category.Name,
    //                ImageUrl = Settings.CanonicalHost + new CategoryImageSettings(category.Id).GetUrl_350px_square().Url,
    //                ItemUrl = Settings.CanonicalHost + Links.CategoryDetail(category.Name, category.Id),
    //                Licence = "CC_BY",
    //                Author = new UserTinyModel(EntityCache.GetUserById(category.Creator != null ? category.Creator.Id : -1)).Name,
    //            })
    //    }, JsonRequestBehavior.AllowGet);
    //    jsonResult.MaxJsonLength = Int32.MaxValue;
    //    return jsonResult;
    //}

    public JsonResult Info()
    {
        return Json(new
        {
            name =  "Memucho",
            url = "https://memucho.de/",
            logo =  "https://memucho.de/Images/Logo/LogoPictogram.png",
            inLanguage = "de",
            type = "Repository",
            description = "Die Beschreibung",
            provider = new  {
                legalName = "Memucho",
                url = "https://memucho.de/",
                email ="team@memucho.de",
                location = new {
                    geo = new {
                        longitude = 13.628557,
                        latitude = 52.315637,
                    }
                }
            },
            startDate = "",
            interfaces = new object[]{ 
                new {
                    url = $"{Settings.CanonicalHost}/api",
                    set = "",
                    metadataPrefix = "",
                    documentation = $"{Settings.CanonicalHost}/api",
                    format = "Json",
                    type = "Generic_Api"
                },
                new {
                    url = $"{Settings.CanonicalHost}/api/edusharing/statistics",
                    set =  "null",
                    metadataPrefix =  "",
                    documentation = "",
                    format = "Json",
                    type = "Statistics"
                }
            },
            isAccessibleForFree = "true"
        }, JsonRequestBehavior.AllowGet);
    }
    [HttpGet]
    public JsonResult Statistics()
    {
        var totalPublicQuestionCount = Sl.R<QuestionRepo>().TotalPublicQuestionCount();
        var totalTopicCount = Sl.R<CategoryRepository>().TotalCategoryCount();

        var total = totalPublicQuestionCount + totalTopicCount;

        var subGroups = new object[]
        {
            new {
                id = "fileFormat",
                count = new object[]
                {
                    new
                    {
                        key = "questions",
                        displayName = "Fragen",
                        count = totalPublicQuestionCount
                    },
                    new
                    {
                        key = "topics",
                        displayName = "Themen",
                        count = totalTopicCount
                    },
                },
            }
        };

        return Json(new
            {
                overall = new {
                    count = total,
                    subGroups = subGroups
                },
                groups = new object[]
                {
                    new
                    {
                        key = "CC_BY",
                        displayName = "Creative Commons",
                        count = total,
                        subGroups
                    }
                },
                user = new
                {
                    count = 0
                }
            }, JsonRequestBehavior.AllowGet
        );
    }
}