using System.Collections.Generic;

public class WelcomeBoxCategoriesMostQModel : BaseModel
{
    public int NumberOfCategories;
    public IEnumerable<Category> Categories;

    public WelcomeBoxCategoriesMostQModel(int numberOfCategories)
    {
        
    }
}
