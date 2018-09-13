using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EasyNetQ.Events;
using TrueOrFalse.Frontend.Web.Code;


public class CategoryAndSetDataWishKnowledge: BaseController
{
    private class Helper
    {
        public int countCategories { get; set; }
        public int countSets { get; set; }
    }

    public List<CategoryAndSetWishKnowledge> filteredCategoryWishKnowledge(ControllerContext controllerContext) 
    {
        var CategorieWishes = GetCategoryData();
        var setWishes = GetSetData();
        var filteredCategoryAndSetWishKnowledges = GetObjectCategoryAndSetWishKnowledges(CategorieWishes, setWishes,controllerContext);
     
        return filteredCategoryAndSetWishKnowledges;
    }

    private ImageFrontendData GetCategoryImage(int CategoryId)
    {
        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(CategoryId, ImageType.Category);
        return new ImageFrontendData(imageMetaData);
    }

    private string KnowledgeWishPartial(Category category, ControllerContext controllerContext)
    {
        var KnowledgeBarPartial = ViewRenderer.RenderPartialView("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(category), controllerContext);

        return KnowledgeBarPartial;
    }

    private string KnowledgeWishPartial(Set set, ControllerContext controllerContext)
    {
        var KnowledgeBarPartial = ViewRenderer.RenderPartialView("~/Views/Sets/Detail/SetKnowledgeBar.ascx", new SetKnowledgeBarModel(set), controllerContext);

        return KnowledgeBarPartial;
    }

    private IList<Category> GetCategoryData()
    {
        var categoryValuationIds = UserValuationCache.GetCategoryValuations(UserId)
            .Where(v => v.IsInWishKnowledge())
            .Select(v => v.CategoryId)
            .ToList();

        return R<CategoryRepository>().GetByIds(categoryValuationIds);
    }

    private IList<Set> GetSetData()
    {
        var setValuationIds = UserValuationCache.GetSetValuations(UserId)
            .Where(v => v.IsInWishKnowledge())
            .Select(v => v.SetId)
            .ToList();
        return R<SetRepo>().GetByIds(setValuationIds);
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
                unSortList.Sort((x, y) => String.CompareOrdinal(x.Titel, y.Titel));
                break;
            case "name|desc":
                unSortList.Sort((x, y) => String.CompareOrdinal(y.Titel, x.Titel));
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

    public List<CategoryAndSetWishKnowledge> GetObjectCategoryAndSetWishKnowledges(IList<Category> CategorieWishes, IList<Set> setWishes, ControllerContext controllerContext)
    {
     
        List<CategoryAndSetWishKnowledge> filteredCategoryAndSetWishKnowledges = new List<CategoryAndSetWishKnowledge>();
      
        foreach (var categoryWish in CategorieWishes)
        {
            var facebookLink = "https://www.facebook.com/sharer/sharer.php?u=https%3A%2F%2F"+ changeUrlToFacebookCompatible(Settings.CanonicalHost + Links.CategoryDetail(categoryWish.Name, categoryWish.Id)) + "%2F&amp;src=sdkpreparse";
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
            categoryAndSetWishKnowledge.EditCategoryOrSetLink = Links.CategoryEdit(categoryWish);
            categoryAndSetWishKnowledge.ShareFacebookLink = facebookLink;
            categoryAndSetWishKnowledge.HasVideo = false;
            categoryAndSetWishKnowledge.KnowledgeWishAVGPercantage = CountDesiredKnowledge(categoryWish);
            categoryAndSetWishKnowledge.AuthorId = categoryWish.Creator.Id;


            filteredCategoryAndSetWishKnowledges.Add(categoryAndSetWishKnowledge);
        }

        foreach (var setWish in setWishes)
        {

            
            var facebookLink = "https://www.facebook.com/sharer/sharer.php?u=https%3A%2F%2F" + changeUrlToFacebookCompatible(Settings.CanonicalHost + Links.SetDetail(setWish.Name, setWish.Id)) + "%2F&amp;src=sdkpreparse";
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
            categoryAndSetWishKnowledge.EditCategoryOrSetLink = Links.SetEdit(setWish);
            categoryAndSetWishKnowledge.ShareFacebookLink = facebookLink;
            categoryAndSetWishKnowledge.HasVideo = setWish.HasVideo;
            categoryAndSetWishKnowledge.KnowledgeWishAVGPercantage = CountDesiredKnowledge(setWish);
            categoryAndSetWishKnowledge.AuthorId = setWish.Creator.Id;

            filteredCategoryAndSetWishKnowledges.Add(categoryAndSetWishKnowledge);
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
        public string ShareFacebookLink { get; set; }
        public bool HasVideo { get; set; }
        public int KnowledgeWishAVGPercantage { get; set; }
        public int AuthorId { get; set; }
    }


    private string  changeUrlToFacebookCompatible(string url)
    {
       return url.Replace("https://", "www.");
    }

    public int GetWuWICountSetAndCategories(bool isAuthor,ControllerContext controllerContext)
    {
        if (isAuthor)
        {
           return  filteredCategoryWishKnowledge(controllerContext).Count;
        }
        return GetCatAndSetList().Count;
    }

    private List<object> GetCatAndSetList()
    {
        var sets = GetSetData();
        var categories = GetCategoryData();
        var listcatsAndSets = new List<object>();
        listcatsAndSets.Add(sets);
        listcatsAndSets.Add(categories);
        return listcatsAndSets;
    }
}
