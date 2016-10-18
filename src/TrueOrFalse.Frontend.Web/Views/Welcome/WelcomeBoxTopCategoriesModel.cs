using System.Collections.Generic;

public class WelcomeBoxTopCategoriesModel : BaseModel
{
    public int NumberOfCategories;
    public IEnumerable<Category> Categories;

    public WelcomeBoxTopCategoriesModel()
    {
    }

    public static WelcomeBoxTopCategoriesModel CreateTopCategories(int amount)
    {
        var result = new WelcomeBoxTopCategoriesModel();
        var categoryRepo = Sl.R<CategoryRepository>();
        result.Categories = categoryRepo.GetWithMostQuestions(amount);

        return result;
    }

    public static WelcomeBoxTopCategoriesModel CreateMostRecent(int amount)
    {
        var result = new WelcomeBoxTopCategoriesModel();
        var categoryRepo = Sl.R<CategoryRepository>();
        result.Categories = categoryRepo.GetMostRecent_WithAtLeast3Questions(amount);

        return result;
    }
}
