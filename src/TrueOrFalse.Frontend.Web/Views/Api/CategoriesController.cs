using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seedworks.Lib;

namespace TrueOrFalse.View.Web.Views.Api
{
    public class CategoriesController : Controller
    {
        // GET: Categories
        [HttpPost]
        [AccessOnlyAsAdmin]
        public JsonResult DeleteDetails(int id)
        {
            var category = EntityCache.GetCategoryCacheItem(id);

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

            var hasDeleted = Sl.CategoryDeleter.Run(category);

            return hasDeleted;
        }

        [AccessOnlyAsLoggedIn]
        [HttpPost]
        public JsonResult GetDeleteData(int id)
        {
            var category = EntityCache.GetCategoryCacheItem(id);
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