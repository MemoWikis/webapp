using System;
using System.Linq;
using System.Web.Mvc;
using Seedworks.Lib;

namespace TrueOrFalse.View.Web.Views.Api
{
    public class CategoriesController : BaseController
    {
        // GET: Categories
        [HttpPost]
        [AccessOnlyAsAdmin]
        public JsonResult DeleteDetails(int id)
        {
            var category = EntityCache.GetCategory(id);

            return new JsonResult
            {
                Data = new
                {
                    categoryTitle = category.Name.WordWrap(50),
                }
            };
        }

        [AccessOnlyAsLoggedIn]
        [HttpPost]
        public CategoryDeleter.HasDeleted Delete(int id)
        {
            var category = Sl.CategoryRepo.GetById(id);
            if (category == null)
                throw new Exception("Category couldn't be deleted. Category with specified Id cannot be found.");

            var parentIds =
                EntityCache.GetCategory(id).ParentCategories().Select(c => c.Id)
                    .ToList(); //if the parents are fetched directly from the category there is a problem with the flush
            var parentCategories = Sl.CategoryRepo.GetByIds(parentIds);

            var hasDeleted = Sl.CategoryDeleter.Run(category, SessionUser.UserId);
            foreach (var parent in parentCategories)
            {
                Sl.CategoryChangeRepo.AddUpdateEntry(parent, SessionUser.UserId, false);
            }

            return hasDeleted;
        }

        [AccessOnlyAsLoggedIn]
        [HttpPost]
        public JsonResult GetDeleteData(int id)
        {
            var category = EntityCache.GetCategory(id);
            var children = EntityCache.GetAllChildren(id);
            var hasChildren = children.Count > 0;
            if (category == null)
                throw new Exception("Category couldn't be deleted. Category with specified Id cannot be found.");

            return new JsonResult
            {
                Data = new
                {
                    CategoryName = category.Name,
                    HasChildren = hasChildren,
                }
            };
        }
    }
}