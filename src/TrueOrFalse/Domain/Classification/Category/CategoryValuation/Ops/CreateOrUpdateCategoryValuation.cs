public class CreateOrUpdateCategoryValuation
{
    public static void Run(int categoryId, int userId, int relevancePeronal = -2)
    {
        var categoryRepo = Sl.CategoryValuationRepo;

        var categoryValuation = categoryRepo.GetBy(categoryId, userId);

        if (categoryValuation == null){
            var newQuestionVal = 
                new CategoryValuation{ CategoryId = categoryId, UserId = userId, RelevancePersonal = relevancePeronal};

            categoryRepo.Create(newQuestionVal);
        }
        else
        {
            if (relevancePeronal != -2)
                categoryValuation.RelevancePersonal = relevancePeronal;

            categoryRepo.Update(categoryValuation);                
        }
        categoryRepo.Flush();
    }
} 