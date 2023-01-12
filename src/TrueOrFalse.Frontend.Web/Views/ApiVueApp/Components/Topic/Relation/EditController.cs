using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;

public class TopicRelationEditController : BaseController
{
    private readonly CategoryRepository _categoryRepository = Sl.CategoryRepo;

    [HttpPost]
    public JsonResult ValidateName(string name)
    {
        var dummyTopic = new Category();
        dummyTopic.Name = name;
        dummyTopic.Type = CategoryType.Standard;
        var topicNameAllowed = new CategoryNameAllowed();
        if (topicNameAllowed.No(dummyTopic))
        {
            var topic = EntityCache.GetCategoryByName(name).FirstOrDefault();
            var url = topic.Visibility == CategoryVisibility.All ? Links.CategoryDetail(topic) : "";
            return Json(new
            {
                categoryNameAllowed = false,
                name,
                url,
                key = "nameIsTaken"
            });
        }

        if (topicNameAllowed.ForbiddenWords(name))   
        {
            return Json(new
            {
                categoryNameAllowed = false,
                name,
                key = "nameIsForbidden"
            });
        }

        return Json(new
        {
            categoryNameAllowed = true
        });
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult QuickCreate(string name, int parentTopicId)
    {
        var topic = new Category(name);
        ModifyRelationsForCategory.AddParentCategory(topic, parentTopicId);

        topic.Creator = Sl.UserRepo.GetById(SessionUser.UserId);
        topic.Type = CategoryType.Standard;
        topic.Visibility = CategoryVisibility.Owner;
        _categoryRepository.Create(topic);

        return Json(new
        {
            success = true,
            url = Links.CategoryDetail(topic),
            id = topic.Id
        });
    }
}