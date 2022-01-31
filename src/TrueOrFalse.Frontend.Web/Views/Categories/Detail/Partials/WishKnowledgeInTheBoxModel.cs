using System.Collections.Generic;
using System.Linq;


public class WishKnowledgeInTheBoxModel : BaseModel
{
    public CategoryCacheItem CategoryCacheItem;


    public WishKnowledgeInTheBoxModel(CategoryCacheItem categoryCacheItem)
    {
        CategoryCacheItem = categoryCacheItem;
    }
    public string Title;
    public string Text;


    public List<Question> GetQuestionToCategories(CategoryCacheItem categoryCacheItem)
    {
        var aggregatedQuestions = new List<Question>();

        var aggregatedCategories = categoryCacheItem.AggregatedCategories(includingSelf: true);

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


        var allQuestionsInAllCategoriesToTheSite = GetQuestionToCategories(CategoryCacheItem);
        allQuestionsInAllCategoriesToTheSite = allQuestionsInAllCategoriesToTheSite.Distinct().ToList();    // remove Duplicate Questions

        var questionValuations = Sl.QuestionValuationRepo.GetByUserFromCache(UserId);                       // Get Question without Questiontext and with WUWI is false
        questionValuations = questionValuations.Where(v => v.IsInWishKnowledge).ToList();             // without WUWi is false
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
}




public class ObjectGetQuestionKnowledge
{
    public int Userid { get; set; }
    public int NumberKnowledgeQuestions { get; set; }
    public List<string> KnowledgeStatus { get; set; }
    public List<Question> AggregatedWishKnowledge { get; set; }
}