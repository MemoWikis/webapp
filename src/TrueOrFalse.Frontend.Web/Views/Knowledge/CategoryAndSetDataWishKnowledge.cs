using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;


public class CategoryAndSetDataWishKnowledge: BaseController
{
    public List<CategoryAndSetWishKnowledge> filteredCategoryWishKnowledge(ControllerContext controllerContext) 
    {
       
       
        var CategorieWishes = GetCategoryData();
        var setWishes = GetSetData();
        var filteredCategoryAndSetWishKnowledges = GetObjectCategoryAndSetWishKnowledges(CategorieWishes, setWishes,controllerContext);
     
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

    public IList<CategoryAndSetWishKnowledge> SortList(List<CategoryAndSetWishKnowledge> unSortList, string sortCondition)
    {
        if (sortCondition.Equals("name|asc"))
        {
            unSortList.Sort((x, y) => String.CompareOrdinal(x.Titel, y.Titel));
        }
        else
        {
        unSortList.Sort((x, y) => String.CompareOrdinal(y.Titel, x.Titel));
        }

        var sortList = unSortList;

        return sortList;
    }

    public List<CategoryAndSetWishKnowledge> GetObjectCategoryAndSetWishKnowledges(IList<Category> CategorieWishes, IList<Set> setWishes, ControllerContext controllerContext)
    {
        List<CategoryAndSetWishKnowledge> filteredCategoryAndSetWishKnowledges = new List<CategoryAndSetWishKnowledge>();

        foreach (var categoryWish in CategorieWishes)
        {
            var categoryAndSetWishKnowledge = new CategoryAndSetWishKnowledge();
            categoryAndSetWishKnowledge.Description = categoryWish.Description;
            categoryAndSetWishKnowledge.Titel = categoryWish.Name;
            categoryAndSetWishKnowledge.ImageFrontendData = GetCategoryImage(categoryWish.Id);
            categoryAndSetWishKnowledge.KnowlegdeWishPartial = KnowledgeWishPartial(categoryWish, controllerContext);
            categoryAndSetWishKnowledge.Id = categoryWish.Id;
            categoryAndSetWishKnowledge.IsCategory = true;
            categoryAndSetWishKnowledge.LinkStartLearningSession = Links.StartCategoryLearningSession(categoryWish.Id);
            categoryAndSetWishKnowledge.DateToLearningTopicLink = Links.DateCreateForCategory(categoryWish.Id).ToString();
            categoryAndSetWishKnowledge.CreateQuestionLink = Links.CreateQuestion(categoryId: categoryWish.Id);
            categoryAndSetWishKnowledge.StartGameLink = Links.GameCreateFromCategory(categoryWish.Id);
            categoryAndSetWishKnowledge.LearnSetsCount = Sl.CategoryRepo.CountAggregatedSets(categoryWish.Id);
            categoryAndSetWishKnowledge.QuestionsCount = Sl.CategoryRepo.CountAggregatedQuestions(categoryWish.Id);
            categoryAndSetWishKnowledge.EditCategoryOrSetLink = Links.CategoryEdit(Sl.CategoryRepo.GetByIdEager(categoryWish.Id));
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
            categoryAndSetWishKnowledge.IsCategory = false;
            categoryAndSetWishKnowledge.LinkStartLearningSession = Links.StartLearningSessionForSet(setWish.Id);
            categoryAndSetWishKnowledge.DateToLearningTopicLink = Links.DateCreateForSet(setWish.Id).ToString();
            categoryAndSetWishKnowledge.CreateQuestionLink = Links.CreateQuestion(setId: setWish.Id);
            categoryAndSetWishKnowledge.StartGameLink = Links.GameCreateFromSet(setWish.Id);
            categoryAndSetWishKnowledge.LearnSetsCount = 1;
            categoryAndSetWishKnowledge.QuestionsCount = Sl.SetRepo.GetByIdEager(setWish.Id).QuestionsInSetPublic.Count ;
            categoryAndSetWishKnowledge.EditCategoryOrSetLink = Links.SetEdit(Sl.SetRepo.GetByIdEager(setWish.Id));
            filteredCategoryAndSetWishKnowledges.Add(categoryAndSetWishKnowledge);
        }

        return filteredCategoryAndSetWishKnowledges;
    }

    public class CategoryAndSetWishKnowledge
    {
        public int Id;
        public string Description;
        public string Titel;
        public ImageFrontendData ImageFrontendData;
        public string KnowlegdeWishPartial;
        public bool IsCategory;
        public string LinkStartLearningSession;
        public string DateToLearningTopicLink { get; set; }
        public string CreateQuestionLink { get; set; }
        public string StartGameLink { get; set; }
        public int LearnSetsCount = 0;
        public int QuestionsCount = 0;
        public string EditCategoryOrSetLink { get; set; }
    }


}
