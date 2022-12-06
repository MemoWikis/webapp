using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;

public class EditTopicController : BaseController
{
    private readonly CategoryRepository _categoryRepository = Sl.CategoryRepo;

    [HttpPost]
    public JsonResult ValidateName(string name)
    {
        var dummyCategory = new Category();
        dummyCategory.Name = name;
        dummyCategory.Type = CategoryType.Standard;
        var categoryNameAllowed = new CategoryNameAllowed();
        if (categoryNameAllowed.No(dummyCategory))
        {
            var category = EntityCache.GetCategoryByName(name).FirstOrDefault();
            var url = category.Visibility == CategoryVisibility.All ? Links.CategoryDetail(category) : "";
            return Json(new
            {
                categoryNameAllowed = false,
                name,
                url,
                key = "nameIsTaken"
            });
        }

        if (categoryNameAllowed.ForbiddenWords(name))   
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
        var category = new Category(name);
        ModifyRelationsForCategory.AddParentCategory(category, parentTopicId);

        category.Creator = Sl.UserRepo.GetById(SessionUser.UserId);
        category.Type = CategoryType.Standard;
        category.Visibility = CategoryVisibility.Owner;
        _categoryRepository.Create(category);

        return Json(new
        {
            success = true,
            url = Links.CategoryDetail(category),
            id = category.Id
        });
    }
}