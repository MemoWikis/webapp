using System.Collections.Generic;

public class CategoryNavigationModel : BaseModel
{
    public Category ActuallCategory;
    public IList<Category> CategoryTrail;

    public CategoryNavigationModel(Category actuallCategory)
    {
        ActuallCategory = actuallCategory;

        if (actuallCategory != default(Category))
            CategoryTrail = GetBreadCrumb.For(actuallCategory);
    }
}