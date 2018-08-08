using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;


public class CategoryAndSetDataWishKnowledge: BaseController
{
    class Helper
    {
        public int countCategories { get; set; }
        public int countSets { get; set; }
    }

    public class QuestionsCount
    {
        public int Solid { get; set; }
        public int NotLearned { get; set; }
        public int NotInWishKnowledge { get; set; }
        public int NeedsConsolidation { get; set; }
        public int NeedsLearned { get; set; }

    }

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
        switch (sortCondition)
        {
            case "name|asc":
                unSortList.Sort((x, y) => String.CompareOrdinal(x.Titel, y.Titel));
                break;
            case "name|desc":
                unSortList.Sort((x, y) => String.CompareOrdinal(y.Titel, x.Titel));
                break;
            case "knowledgeBar|asc":
                unSortList.Sort((a, b) =>
                {
                    int result = b.KnowledgeWishQuestionCount.Solid.CompareTo(a.KnowledgeWishQuestionCount.Solid);
                    if (result != 0)
                    {
                        return result;
                    }
                   
                        int result1 = b.KnowledgeWishQuestionCount.NeedsConsolidation.CompareTo(a.KnowledgeWishQuestionCount.NeedsConsolidation);
                        if (result1 != 0)
                        {
                            return result1;
                        }
                            return b.KnowledgeWishQuestionCount.NotLearned.CompareTo(a.KnowledgeWishQuestionCount.NotLearned);
                });
                break;
            case "knowledgeBar|desc":
                unSortList.Sort((a, b) =>
                {
                    int result = a.KnowledgeWishQuestionCount.Solid.CompareTo(b.KnowledgeWishQuestionCount.Solid);
                    return (result != 0)
                        ? result
                        : 1 * a.KnowledgeWishQuestionCount.NeedsConsolidation.CompareTo(b.KnowledgeWishQuestionCount.NeedsConsolidation);
                });
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
            categoryAndSetWishKnowledge.KnowledgeWishQuestionCount = CountDesiredKnowledge(categoryWish);


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
            categoryAndSetWishKnowledge.KnowledgeWishQuestionCount = CountDesiredKnowledge(setWish);

            filteredCategoryAndSetWishKnowledges.Add(categoryAndSetWishKnowledge);
        }

        return filteredCategoryAndSetWishKnowledges;
    }

    private QuestionsCount CountDesiredKnowledge(object categoryOrSet)
    {
        var questionsCount = new QuestionsCount();

        if (categoryOrSet is Category)
        {
            var category = (Category) categoryOrSet;
            var categoryModel = new CategoryKnowledgeBarModel(category);
           
            questionsCount.NeedsConsolidation = categoryModel.CategoryKnowledgeSummary.NeedsLearning;
            questionsCount.Solid = categoryModel.CategoryKnowledgeSummary.Solid;
            questionsCount.NotLearned = categoryModel.CategoryKnowledgeSummary.NotLearned;
            questionsCount.NotInWishKnowledge = categoryModel.CategoryKnowledgeSummary.NotInWishknowledge;

            return questionsCount;
        }

        if (categoryOrSet is Set)
        {
            var set = (Set)categoryOrSet;
            
            var setKnowledge = KnowledgeSummaryLoader.Run(UserId, set);
            questionsCount.NeedsConsolidation = setKnowledge.NeedsConsolidation  ;
            questionsCount.Solid = setKnowledge.Solid;
            questionsCount.NotLearned = setKnowledge.NotLearned;
            questionsCount.NotInWishKnowledge = setKnowledge.NotInWishknowledge;
            questionsCount.NeedsLearned = setKnowledge.NeedsLearning;

            return questionsCount;
        }

        questionsCount.NeedsConsolidation = -1;
        questionsCount.Solid = -1;
        questionsCount.NotLearned = -1;
        questionsCount.NotInWishKnowledge = -1;

        return questionsCount;
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
        public QuestionsCount KnowledgeWishQuestionCount { get; set; }
    }


    public string  changeUrlToFacebookCompatible(string url)
    {
       return url.Replace("https://", "www.");
    }

    public  List<int> GetWuWICountSetAndCategories()
    {
        List<int> CountSetsandCategories = new List<int>();
        var helper = new Helper();
        helper.countCategories = GetCategoryData().Count;
        helper.countSets = GetSetData().Count;
        CountSetsandCategories.Add(helper.countSets);
        CountSetsandCategories.Add(helper.countCategories);

        return CountSetsandCategories;
    }
}
