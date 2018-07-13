using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


public class CategoryAndSetDataWishKnowledge: BaseController
{
    public List<CategoryWishKnowledge> filteredCategoryWishKnowledge(ControllerContext controllerContext)
    {
        List<CategoryWishKnowledge> filteredCategoryWishKnowledges = new List<CategoryWishKnowledge>();

        var categoriesWish = GetCategoryData();

        foreach (var categoryWish in categoriesWish)
        {
            var categoryWishKnowledge = new CategoryWishKnowledge();
            categoryWishKnowledge.CategoryDescription = categoryWish.Description;
            categoryWishKnowledge.CategoryTitel = categoryWish.Name;
            categoryWishKnowledge.ImageFrontendData = GetCategoryImage(categoryWish.Id);
            categoryWishKnowledge.KnowlegdeWishPartial = KnowledgeWishPartial(categoryWish, controllerContext);
            categoryWishKnowledge.CategoryId = categoryWish.Id;
            filteredCategoryWishKnowledges.Add(categoryWishKnowledge);
        }

        return filteredCategoryWishKnowledges;
    }

    public ImageFrontendData GetCategoryImage(int CategoryId)
    {
        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(CategoryId, ImageType.Category);
        return new ImageFrontendData(imageMetaData);
    }

    public string KnowledgeWishPartial(Category category, ControllerContext controllerContext)
    {
        var KnowledgeBarPartial = ViewRenderer.RenderPartialView("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(category), controllerContext);

        return KnowledgeBarPartial;

    }

    public IList<Category> GetCategoryData()
    {
        var categoryValuationIds = UserValuationCache.GetCategoryValuations(UserId)
            .Where(v => v.IsInWishKnowledge())
            .Select(v => v.CategoryId)
            .ToList();

        return R<CategoryRepository>().GetByIds(categoryValuationIds);

    }
    public class CategoryWishKnowledge
    {
        public int CategoryId;
        public string CategoryDescription;
        public string CategoryTitel;
        public ImageFrontendData ImageFrontendData;
        public string KnowlegdeWishPartial;

    }
}
