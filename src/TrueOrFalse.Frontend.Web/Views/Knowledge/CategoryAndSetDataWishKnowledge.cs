using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;



public class CategoryAndSetDataWishKnowledge: BaseController
{
    readonly IList<Category> Categories;
    readonly IList<ImageMetaData> CategoryFrontendImageDataIList;

    IList<Set> Sets;
    readonly IList<ImageMetaData> SetFrontendImageDataIList;

    public CategoryAndSetDataWishKnowledge()
    {
       var categoriesIds = UserValuationCache.GetCategoryValuations(UserId)  
            .Where(v => v.IsInWishKnowledge())
            .Select(i => i.CategoryId)
            .ToList();
        Categories = EntityCache.GetCategories(categoriesIds).ToList();
        CategoryFrontendImageDataIList = Sl.ImageMetaDataRepo.GetBy(categoriesIds, ImageType.Category);

        var setIds = UserValuationCache.GetSetValuations(UserId) 
            .Where(v => v.IsInWishKnowledge())
            .Select(i => i.SetId)
            .ToList();
        Sets = EntityCache.GetSetsByIds(setIds);
        SetFrontendImageDataIList = Sl.ImageMetaDataRepo.GetBy(setIds, ImageType.QuestionSet);
    }

    //----------------------------------------------------------------------------------- 
    public List<CategoryAndSetWishKnowledge> filteredCategoryWishKnowledge(ControllerContext controllerContext)
    {   
        return GetObjectCategoryAndSetWishKnowledges(Categories, Sets, controllerContext);  // 1300 ms wau 
    }

    private string KnowledgeWishPartial(Category category, ControllerContext controllerContext)
    {
        var KnowledgeBarPartial = ViewRenderer.RenderPartialView("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(category), controllerContext); // 128 ms
        
        return KnowledgeBarPartial;
    }

    private string KnowledgeWishPartial(Set set, ControllerContext controllerContext)
    {
        var KnowledgeBarPartial = ViewRenderer.RenderPartialView("~/Views/Sets/Detail/SetKnowledgeBar.ascx", new SetKnowledgeBarModel(set), controllerContext);

        return KnowledgeBarPartial;
    }
   
    public IList<CategoryAndSetWishKnowledge> SortList(List<CategoryAndSetWishKnowledge> unSortList, string sortCondition,bool isAuthor= false) 
    {
        if (isAuthor)
        {
            var isAuthorListUnsorted = new List<CategoryAndSetWishKnowledge>();
            foreach (var uSL in unSortList)
            {
                if (uSL.AuthorId == UserId)
                {
                    isAuthorListUnsorted.Add(uSL);
                }
            }
            unSortList = isAuthorListUnsorted;
        }
        switch (sortCondition)
        {
            case "name|asc":
                unSortList.Sort((x, y) => String.CompareOrdinal(x.Title, y.Title));
                break;
            case "name|desc":
                unSortList.Sort((x, y) => String.CompareOrdinal(y.Title, x.Title));
                break;
            case "knowledgeBar|asc":
                unSortList.Sort((x,y)=> y.KnowledgeWishAVGPercantage.CompareTo(x.KnowledgeWishAVGPercantage));
                break;
            case "knowledgeBar|desc":
                unSortList.Sort((x, y) => -1 * y.KnowledgeWishAVGPercantage.CompareTo(x.KnowledgeWishAVGPercantage));
                break;
        }

        var sortList = unSortList;

        return sortList;
    }

    public List<CategoryAndSetWishKnowledge> GetObjectCategoryAndSetWishKnowledges(IList<Category> CategorieWishes,  IList<Set> setWishes, ControllerContext controllerContext)
    {
        List<CategoryAndSetWishKnowledge> filteredCategoryAndSetWishKnowledges = new List<CategoryAndSetWishKnowledge>();
        var counterCategoryImage = 0;
        var counterSetImage = 0;
        foreach (var categoryWish in CategorieWishes)
        {
            var facebookLink = "https://www.facebook.com/sharer/sharer.php?u=https%3A%2F%2F"+ changeUrlToFacebookCompatible(Settings.CanonicalHost + Links.CategoryDetail(categoryWish.Name, categoryWish.Id)) + "%2F&amp;src=sdkpreparse";
            var categoryAndSetWishKnowledge = new CategoryAndSetWishKnowledge
            {
                Description = categoryWish.Description,
                Title = categoryWish.Name,
                ImageFrontendData = CategoryFrontendImageDataIList[counterCategoryImage],
                KnowlegdeWishPartial = KnowledgeWishPartial(categoryWish, controllerContext),
                Id = categoryWish.Id,
                IsCategory = true,
                LinkStartLearningSession = Links.StartCategoryLearningSession(categoryWish.Id),
                DateToLearningTopicLink = Links.DateCreateForCategory(categoryWish.Id).ToString(),
                CreateQuestionLink = Links.CreateQuestion(categoryId: categoryWish.Id),
                StartGameLink = Links.GameCreateFromCategory(categoryWish.Id),
                LearnSetsCount = categoryWish.CountSets,
                QuestionsCount = categoryWish.CountQuestionsAggregated,
                EditCategoryOrSetLink = Links.CategoryEdit(categoryWish),
                ShareFacebookLink = facebookLink,
                HasVideo = false,
                KnowledgeWishAVGPercantage = CountDesiredKnowledge(categoryWish),
                AuthorId = categoryWish.Creator.Id,
                LinkToSetOrCategory = Links.GetUrl(categoryWish)
            };

            filteredCategoryAndSetWishKnowledges.Add(categoryAndSetWishKnowledge);
            counterCategoryImage++;
        }

        foreach (var setWish in setWishes)
        {
            var facebookLink = "https://www.facebook.com/sharer/sharer.php?u=https%3A%2F%2F" + changeUrlToFacebookCompatible(Settings.CanonicalHost + Links.SetDetail(setWish.Name, setWish.Id)) + "%2F&amp;src=sdkpreparse";
            var categoryAndSetWishKnowledge = new CategoryAndSetWishKnowledge();

            categoryAndSetWishKnowledge.Description = setWish.Text;
            categoryAndSetWishKnowledge.Title = setWish.Name;
            categoryAndSetWishKnowledge.ImageFrontendData = SetFrontendImageDataIList[counterSetImage];
            categoryAndSetWishKnowledge.KnowlegdeWishPartial = KnowledgeWishPartial(setWish, controllerContext);
            categoryAndSetWishKnowledge.Id = setWish.Id;
            categoryAndSetWishKnowledge.IsCategory = false;
            categoryAndSetWishKnowledge.LinkStartLearningSession = Links.StartLearningSessionForSet(setWish.Id);
            categoryAndSetWishKnowledge.DateToLearningTopicLink = Links.DateCreateForSet(setWish.Id).ToString();
            categoryAndSetWishKnowledge.CreateQuestionLink = Links.CreateQuestion(setId: setWish.Id);
            categoryAndSetWishKnowledge.StartGameLink = Links.GameCreateFromSet(setWish.Id);
            categoryAndSetWishKnowledge.LearnSetsCount = 1;
            categoryAndSetWishKnowledge.QuestionsCount = setWish.QuestionCount() ;
            categoryAndSetWishKnowledge.EditCategoryOrSetLink = Links.SetEdit(setWish);
            categoryAndSetWishKnowledge.ShareFacebookLink = facebookLink;
            categoryAndSetWishKnowledge.HasVideo = setWish.HasVideo;
            categoryAndSetWishKnowledge.KnowledgeWishAVGPercantage = CountDesiredKnowledge(setWish);
            categoryAndSetWishKnowledge.AuthorId = setWish.Creator.Id;
            categoryAndSetWishKnowledge.LinkToSetOrCategory = Links.GetUrl(setWish);

            filteredCategoryAndSetWishKnowledges.Add(categoryAndSetWishKnowledge);
            counterSetImage++;
        }
        return filteredCategoryAndSetWishKnowledges;
    }

    private int CountDesiredKnowledge(object categoryOrSet)
    {
        var solid = 0;
        var needsConsolidation = 0;
        var notLearned = 0;
        var notInWishKnowledge = 0;
        var needsLearning = 0;
        var questionAVGPercantage = 0;
        var questionTotalNumber = 0;

        if (categoryOrSet is Category)
        {
            var category = (Category) categoryOrSet;
            var categoryKnowledge = KnowledgeSummaryLoader.RunFromMemoryCache(category.Id, UserId);

            solid = categoryKnowledge.Solid;
            needsConsolidation = categoryKnowledge.NeedsConsolidation;
            needsLearning = categoryKnowledge.NeedsLearning;
            notLearned = categoryKnowledge.NotLearned;
            notInWishKnowledge = categoryKnowledge.NotInWishknowledge;
            
        }else if (categoryOrSet is Set)
        {
            var set = (Set)categoryOrSet;
            var setKnowledge = KnowledgeSummaryLoader.Run(UserId, set);
            
            solid = setKnowledge.Solid;
            needsConsolidation = setKnowledge.NeedsConsolidation;
            notLearned = setKnowledge.NotLearned;
            needsLearning = setKnowledge.NeedsLearning;
            notInWishKnowledge = setKnowledge.NotInWishknowledge;
        }
        else
        {
            // error Handling
            return -1;
        }

        questionTotalNumber = solid + needsConsolidation + needsLearning + notInWishKnowledge + notLearned;
        questionAVGPercantage = ((solid * 100) + (needsConsolidation * 50) + (needsLearning * 20) + (notLearned * 2)) / questionTotalNumber;
        return questionAVGPercantage;
    }

    public class CategoryAndSetWishKnowledge
    {
        public int Id;
        public string Description;
        public string Title;
        public ImageMetaData ImageFrontendData;
        public string KnowlegdeWishPartial;
        public bool IsCategory;
        public string LinkStartLearningSession;
        public string DateToLearningTopicLink { get; set; }
        public string CreateQuestionLink { get; set; }
        public string StartGameLink { get; set; }
        public int LearnSetsCount = 0;
        public int QuestionsCount = 0;
        public string EditCategoryOrSetLink { get; set; }
        public string ShareFacebookLink { get; set; }
        public bool HasVideo { get; set; }
        public int KnowledgeWishAVGPercantage { get; set; }
        public int AuthorId { get; set; }
        public string LinkToSetOrCategory { get; set; }
    }

    private string  changeUrlToFacebookCompatible(string url)
    {
       return url.Replace("https://", "www.");
    }

    //public int GetWuWICountSetAndCategories(bool isAuthor,ControllerContext controllerContext)
    //{
    //    if (isAuthor)
    //    {
    //       return  filteredCategoryWishKnowledge(controllerContext).Count;
    //    }
    //    return GetCatAndSetList().Count;
    //}

    //private List<object> GetCatAndSetList()
    //{
    //    var sets = Sets;
    //    var categories = Categories;
    //    var listcatsAndSets = new List<object>();
    //    listcatsAndSets.Add(sets);
    //    listcatsAndSets.Add(categories);
    //    return listcatsAndSets;
    //}
}
