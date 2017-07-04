using System.Collections.Generic;

public class CategoryNavigationModel : BaseModel
{
    public Category ActuallCategory;

    public IList<Category> CategoryTrail;

    public List<Category> DefaultCategoriesList;

    public CategoryNavigationModel(Category actuallCategory)
    {
        ActuallCategory = actuallCategory;

        FillDefaultCategoriesList();

        if (actuallCategory != default(Category))
            CategoryTrail = GetBreadCrumb.For(actuallCategory);
    }

    private void FillDefaultCategoriesList()
    {
        DefaultCategoriesList = new List<Category>
        {
            //Sl.CategoryRepo.GetByName("Schule").First(),
            Sl.CategoryRepo.GetById(640),
            Sl.CategoryRepo.GetById(151),
            Sl.CategoryRepo.GetById(689),
            Sl.CategoryRepo.GetById(709)
        };
    }
}