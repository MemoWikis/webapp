using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;

public class WishKnowledgeInTheBoxModel : BaseModel
{
    private ReturnToJson returnToJson;
   

    public WishKnowledgeInTheBoxModel()
    {
       returnToJson = new ReturnToJson();
     
    }


    public JsonResult GetObjectQuestionKnowledge()
    {
        return returnToJson.BuildObjectGetQuestionKnowledge();
    }


}


public class ReturnToJson : BaseController
{

    public Category Category;

    public string Title;
    public string Text;

    public List<Category> CategoryList;

    public bool HasUsedOrderListWithLoadList;
    public List<Question> test2(int UserId, Category category)
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

    public JsonResult BuildObjectGetQuestionKnowledge()
    {
        // ---------------- Properties -------------------
        var getQuestionKnowledge = new ObjectGetQuestionKnowledge();
        var aggregateWishKnowledge = new List<Question>();
        var knowledgeStatus = new List<string>();

        // ---------- Evaluation ----------
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
        var questionValuations = Sl.QuestionValuationRepo.GetByUserFromCache(UserId);
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
        getQuestionKnowledge.NumberKnowledgeQuestions = aggregateWishKnowledge.Count;
        getQuestionKnowledge.KnowledgeStatus = knowledgeStatus;
        getQuestionKnowledge.AggregatedWishKnowledge = aggregateWishKnowledge;



        return Json(getQuestionKnowledge, JsonRequestBehavior.AllowGet);
    }

}


public class ObjectGetQuestionKnowledge
{
    public int Userid { get; set; }
    public int NumberKnowledgeQuestions { get; set; }
    public List<string> KnowledgeStatus { get; set; }
    public List<Question> AggregatedWishKnowledge { get; set; }
} 