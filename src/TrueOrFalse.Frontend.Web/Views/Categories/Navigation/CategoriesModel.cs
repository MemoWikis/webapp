using System.Collections.Generic;
using System.Linq;

public class CategoryNavigationModel : BaseModel
{
    public Category ActuallCategory;
    public Category RootCategory;

    public List<Category> CategoryTrail;

    public List<Category> DefaultCategoriesList;

    public CategoryNavigationModel(Category actuallCategory)
    {
        ActuallCategory = actuallCategory;

        FillDefaultCategoriesList();

        if (actuallCategory != null)
        {
            CategoryTrail = GetBreadCrumb.For(actuallCategory).ToList();
            SetRootCategory();
            CategoryTrail.Reverse();
        }
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

    private void SetRootCategory()
    {
        if (CategoryTrail.Count > 0)
        {
            RootCategory = CategoryTrail.First();
            CategoryTrail.RemoveAt(0);
        }
        else
        {
            RootCategory = 
                DefaultCategoriesList.Contains(ActuallCategory)
                ? ActuallCategory
                : Sl.CategoryRepo.GetById(709);
        }
    }
}