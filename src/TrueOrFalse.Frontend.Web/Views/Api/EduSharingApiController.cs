using System;
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
            Creator = category.Creator.Name,
            CreatorMetaData = category.Creator.Name,
            Url = Settings.CanonicalHost + Links.CategoryDetail(category.Name, category.Id),
            Thumbnail = Settings.CanonicalHost + new CategoryImageSettings(category.Id).GetUrl(300).Url
        }, JsonRequestBehavior.AllowGet);
    }


    public JsonResult Search(string term, int pageSize = 5)
    {
        var result = Sl.SearchCategories.Run(term, new Pager { PageSize = pageSize });

        return Json(new
        {
            ResultCount = result.Count,
            Items = result
                .GetCategories()
                .Select(category => new
                {
                    TopicId = category.Id,
                    Name = category.Name,
                    ImageUrl = Settings.CanonicalHost + new CategoryImageSettings(category.Id).GetUrl_350px_square(),
                    ItemUrl = Settings.CanonicalHost + Links.CategoryDetail(category.Name, category.Id),
                    Licence = "CC_BY",
                    Author = category.Creator.Name
                })
        }, JsonRequestBehavior.AllowGet);
    }

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
                    url = $"{Settings.CanonicalHost}api",
                    set = "",
                    metadataPrefix = "",
                    documentation = $"{Settings.CanonicalHost}api",
                    format = "Json",
                    type = "Generic_Api"
                },
                new {
                    url = $"{Settings.CanonicalHost}api/edusharing/statistics",
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

    public JsonResult Statistics()
    {
        //var allCategories = EntityCache.GetCategories();
        //var allQuestions = EntityCache.Que

        var questionCount = R<QuestionGetCount>().Run();
        

        return Json(new
        {
            overall =  22,
            id = "fileformat",
            count = new object[]
            {
                new {}
            }
        });
    }
}