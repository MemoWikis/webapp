using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Helpers;
using System.Web.Mvc;

public class WishKnowledgeInTheBoxModel : BaseModel
{
    public Category CategoryList;
    public bool HasUsedOrderListWithLoadList;


    public WishKnowledgeInTheBoxModel(Category category)
    {
        CategoryList = category;
        
    }
    public string Title;
    public string Text;


    public List<Question> GetQuestionToCategories(int UserId, Category category)
    {
        var aggregatedQuestions = new List<Question>();

        var aggregatedCategories = category.AggregatedCategories(includingSelf: true);

        foreach (var currentCategory in aggregatedCategories)
        {
            aggregatedQuestions.AddRange(EntityCache.GetQuestionsForCategory(currentCategory.Id));
        }

        return aggregatedQuestions;
    }

    public ObjectGetQuestionKnowledge BuildObjectGetQuestionKnowledge()
    {
        var aggregateWishKnowledge = new List<Question>();
        var knowledgeStatus = new List<string>();
        var getQuestionKnowledge = new ObjectGetQuestionKnowledge();


        var allQuestionsInAllCategoriesToTheSite = GetQuestionToCategories(UserId, CategoryList);
        allQuestionsInAllCategoriesToTheSite = allQuestionsInAllCategoriesToTheSite.Distinct().ToList();    // remove Duplicate Questions

        var questionValuations = Sl.QuestionValuationRepo.GetByUserFromCache(UserId);                       // Get Question without Questiontext and with WUWI is false
        questionValuations = questionValuations.Where(v => v.RelevancePersonal != -1).ToList();             // without WUWi is false
        questionValuations = questionValuations.OrderByDescending(o => o.KnowledgeStatus).ToList();
        for (var i = 0; i < questionValuations.Count; i++)
        {

            for (var j = 0; j < allQuestionsInAllCategoriesToTheSite.Count; j++)
            {
                if (questionValuations.ElementAt(i).Question.Id == allQuestionsInAllCategoriesToTheSite.ElementAt(j).Id)
                {
                    aggregateWishKnowledge.Add(allQuestionsInAllCategoriesToTheSite.ElementAt(j));
                    knowledgeStatus.Add(questionValuations.ElementAt(i).KnowledgeStatus.ToString());
                }
            }
        }

      
        //    //------ Zuweisung--------
        // og.Userid = UserId;
        getQuestionKnowledge.NumberKnowledgeQuestions = aggregateWishKnowledge.Count;
        getQuestionKnowledge.KnowledgeStatus = knowledgeStatus;
        getQuestionKnowledge.AggregatedWishKnowledge = aggregateWishKnowledge;

        

        //    return getQuestionKnowledge;
        //}
        return getQuestionKnowledge;
    }
    //    // ---------------- Properties -------------------
    //    var getQuestionKnowledge = new ObjectGetQuestionKnowledge();
    //    var aggregateWishKnowledge = new List<Question>();
    //    var knowledgeStatus = new List<string>();

    //    // ---------- Evaluation ----------
    //    var aggregatedQuestions = new List<Question>();
    //    foreach (Category category in CategoryList)
    //    {
    //        var temp1 = GetQuestionToCategories(UserId, category);

    //        foreach (var f in temp1)
    //        {
    //            aggregatedQuestions.Add(f);
    //        }

    //    }

    //    aggregatedQuestions = aggregatedQuestions.Distinct().ToList();
    //    var questionValuations = Sl.QuestionValuationRepo.GetByUserFromCache(UserId);
    //    questionValuations = questionValuations.Where(v => v.RelevancePersonal != -1).ToList();

    //    for (var i = 0; i < questionValuations.Count; i++)
    //    {

    //        for (var j = 0; j < aggregatedQuestions.Count; j++)
    //        {
    //            if (questionValuations.ElementAt(i).Question.Id == aggregatedQuestions.ElementAt(j).Id)
    //            {
    //                aggregateWishKnowledge.Add(aggregatedQuestions.ElementAt(j));
    //                knowledgeStatus.Add(questionValuations.ElementAt(i).KnowledgeStatus.ToString());
    //            }
    //        }
    //    }

    //    //------ Zuweisung--------
    //    // og.Userid = UserId;
    //    getQuestionKnowledge.NumberKnowledgeQuestions = aggregateWishKnowledge.Count;
    //    getQuestionKnowledge.KnowledgeStatus = knowledgeStatus;
    //    getQuestionKnowledge.AggregatedWishKnowledge = aggregateWishKnowledge;



    //    return getQuestionKnowledge;
    //}







}




public class ObjectGetQuestionKnowledge
{
    public int Userid { get; set; }
    public int NumberKnowledgeQuestions { get; set; }
    public List<string> KnowledgeStatus { get; set; }
    public List<Question> AggregatedWishKnowledge { get; set; }
}