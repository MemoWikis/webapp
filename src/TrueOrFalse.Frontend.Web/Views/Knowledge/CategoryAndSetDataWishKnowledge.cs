using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


public class CategoryAndSetDataWishKnowledge: BaseController
{
    public List<CategoryAndSetWishKnowledge> filteredCategoryWishKnowledge(ControllerContext controllerContext)
    {
        List<CategoryAndSetWishKnowledge> filteredCategoryAndSetWishKnowledges = new List<CategoryAndSetWishKnowledge>();

        var CategorieWishes = GetCategoryData();
        var setWishes = GetSetData();
        foreach (var categoryWish in CategorieWishes)
        {
            var categoryAndSetWishKnowledge = new CategoryAndSetWishKnowledge();
            categoryAndSetWishKnowledge.Description = categoryWish.Description;
            categoryAndSetWishKnowledge.Titel = categoryWish.Name;
            categoryAndSetWishKnowledge.ImageFrontendData = GetCategoryImage(categoryWish.Id);
            categoryAndSetWishKnowledge.KnowlegdeWishPartial = KnowledgeWishPartial(categoryWish, controllerContext);
            categoryAndSetWishKnowledge.Id = categoryWish.Id;
            categoryAndSetWishKnowledge.isCategory = true;
            filteredCategoryAndSetWishKnowledges.Add(categoryAndSetWishKnowledge);
            
        }

        foreach (var setWish in setWishes)
        {
            var categoryAndSetWishKnowledge = new CategoryAndSetWishKnowledge();
            categoryAndSetWishKnowledge.Description = setWish.Text;
            categoryAndSetWishKnowledge.Titel = setWish.Name;
            categoryAndSetWishKnowledge.ImageFrontendData = GetCategoryImage(setWish.Id);
            categoryAndSetWishKnowledge.KnowlegdeWishPartial = KnowledgeWishPartial(setWish, controllerContext);
            categoryAndSetWishKnowledge.Id = setWish.Id;
            categoryAndSetWishKnowledge.isCategory = false;
            filteredCategoryAndSetWishKnowledges.Add(categoryAndSetWishKnowledge);

        }

        return filteredCategoryAndSetWishKnowledges;
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
    public string KnowledgeWishPartial(Set set, ControllerContext controllerContext)
    {
        var KnowledgeBarPartial = ViewRenderer.RenderPartialView("~/Views/Sets/Detail/SetKnowledgeBar.ascx", new SetKnowledgeBarModel(set), controllerContext);

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

    public IList<Set> GetSetData()
    {
        var setValuationIds = UserValuationCache.GetSetValuations(UserId)
            .Where(v => v.IsInWishKnowledge())
            .Select(v => v.SetId)
            .ToList();
        return R<SetRepo>().GetByIds(setValuationIds);

    }
    public class CategoryAndSetWishKnowledge
    {
        
        public int Id;
        public string Description;
        public string Titel;
        public ImageFrontendData ImageFrontendData;
        public string KnowlegdeWishPartial;
        public bool isCategory; 

    }

  
}
