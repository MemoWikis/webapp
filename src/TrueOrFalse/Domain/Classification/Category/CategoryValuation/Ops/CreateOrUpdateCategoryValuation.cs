public class CreateOrUpdateCategoryValuation
{
    public static void Run(int categoryId, int userId, int relevancePeronal = -2)
    {
        var categoryValuationRepo = Sl.CategoryValuationRepo;

        var categoryValuation = categoryValuationRepo.GetBy(categoryId, userId);

        if (categoryValuation == null)
        {
            var newQuestionVal =
                new CategoryValuation { CategoryId = categoryId, UserId = userId, RelevancePersonal = relevancePeronal };

            categoryValuationRepo.Create(newQuestionVal);
        }
        else
        {
            if (relevancePeronal != -2)
                categoryValuation.RelevancePersonal = relevancePeronal;

            categoryValuationRepo.Update(categoryValuation);
        }

        categoryValuationRepo.Flush();
    }
}