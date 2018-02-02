using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Ajax.Utilities;
using NHibernate.Criterion;


public class TopicNavigationModel : BaseModel
{
    public Category Category;

    public string Title;
    public string Text;
    public KnowledgeSummary SetKnowledgeSummary;

    public List<Category> CategoryList;

    public bool HasUsedOrderListWithLoadList;

    public TopicNavigationModel(Category category, string title, string text = null, string load = null, string order = null)
    {
     
        Category = category;

        var isLoadList = false;
        switch (load)
        {
            case null:
            case "All":
                CategoryList = Sl.CategoryRepo.GetChildren(category.Id).ToList();
                break;

            default:
                var categoryIdList = load.Split(',').ToList().ConvertAll(Int32.Parse);
                CategoryList = ConvertToCategoryList(categoryIdList);
                isLoadList = true;
                break;
        }

        switch (order)
        {
            case null:
            case "QuestionAmount":
                if(load == null || load == "All")
                    CategoryList = CategoryList.OrderByDescending(c => c.GetAggregatedQuestionsFromMemoryCache().Count).ToList();
                break;

            case "Name":
                CategoryList = CategoryList.OrderBy(c => c.Name).ToList();
                break;

            default:
                if (isLoadList)
                {
                    throw new Exception("\"Load: \" und \"Order: \" können nicht gleichzeitig mit Category-Id-Listen als Parameter verwendet werden!");
                }
                var firstCategories = ConvertToCategoryList(order.Split(',').ToList().ConvertAll(Int32.Parse));
                CategoryList = OrderByCategoryList(firstCategories);
                break;
        }


        CategoryList = CategoryList.Where(c => c.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Standard).ToList();

        Title = title;
        Text = text;
    }


    public List<Question> test2(int UserId, Category category )
    {

        var aggregatedQuestions = new List<Question>();

        var aggregatedCategories = category.AggregatedCategories(includingSelf: true);
  
        foreach (var currentCategory in aggregatedCategories)
        {
            aggregatedQuestions.AddRange(EntityCache.GetQuestionsForCategory(currentCategory.Id));
        }

        var aggregatedSets = EntityCache.GetSetsForCategories(aggregatedCategories);

        foreach (var set in aggregatedSets)
        {
            aggregatedQuestions.AddRange(set.Questions());
        }

        return aggregatedQuestions;
    }



    public ObjectGetQuestionKnowledge  BuildObjectGetQuestionKnowledge()
    {

        // ----------------Eigenschaften -------------------
        ObjectGetQuestionKnowledge og = new ObjectGetQuestionKnowledge();
        IList<QuestionValuation> questionValuations = new List<QuestionValuation>();
        List<Category> c = new List<Category>();
        List<Question> aggregateWishKnowledge = new List<Question>();
        List<String> knowledgeStatus = new List<string>();

        // ---------- Auswertung ----------
        var aggregatedQuestions = new List<Question>();
        foreach (var category in CategoryList)
        {
            var temp1 = test2(UserId, category);

            foreach (var f in temp1)
            {
                aggregatedQuestions.Add(f);
            }
  
        }

        aggregatedQuestions = aggregatedQuestions.Distinct().ToList();
        questionValuations = Sl.QuestionValuationRepo.GetByUserFromCache(UserId);
        questionValuations = questionValuations.Where(v => v.RelevancePersonal != -1).ToList();

        for (var i = 0; i < questionValuations.Count; i++)
        {

            for (var j = 0; j < aggregatedQuestions.Count; j++)
            {
                if (questionValuations.ElementAt(i).Question.Id == aggregatedQuestions.ElementAt(j).Id)
                {
                    aggregateWishKnowledge.Add(aggregatedQuestions.ElementAt(j));
                    knowledgeStatus.Add(questionValuations.ElementAt(i).KnowledgeStatus.ToString());
                }
            }
        }

       
      
        //------ Zuweisung--------
       // og.Userid = UserId;
        og.NumberKnowledgeQuestions = aggregateWishKnowledge.Count;
        og.KnowledgeStatus = knowledgeStatus;
        
        return og;
    }

    public int GetTotalQuestionCount(Category category)
    {
        return category.GetAggregatedQuestionsFromMemoryCache().Count;
    }

    public int GetTotalSetCount(Category category)
    {
        return category.GetAggregatedSetsFromMemoryCache().Count;
    }

    public ImageFrontendData GetCategoryImage(Category category)
    {
        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(category.Id, ImageType.Category);
        return new ImageFrontendData(imageMetaData);
    }

    private List<Category> ConvertToCategoryList(List<int> categoryIdList)
    {
        var categoryList = new List<Category>();
        foreach (var categoryId in categoryIdList)
        {
            var category = Sl.CategoryRepo.GetById(categoryId);
            categoryList.Add(category);
        }

        return categoryList;
    }

    private List<Category> OrderByCategoryList(List<Category> firstCategories)
    {
        foreach (var category in firstCategories)
        {
            CategoryList.Remove(category);
        }

        firstCategories.AddRange(CategoryList);
        return firstCategories;
    }

    public static String returnKnowledgeStatus(List<String> list, int counter)
    {  
        return list.ElementAt(counter);
    }
}


public class ObjectGetQuestionKnowledge
{
    public int Userid { get; set; }
    public int NumberKnowledgeQuestions { get; set; }
    public List<String> KnowledgeStatus { get; set; }
}



