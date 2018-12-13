using System.Linq;

public class InitializeCategoryChanges
{
    public static void Go()
    {
        var allCategories = Sl.CategoryRepo.GetAll();
        var allCategoryChanges = Sl.CategoryChangeRepo.GetAllEager();

        var memuchoUser = Sl.UserRepo.GetMemuchoUser();

        foreach(var category in allCategories)
        {
            if (allCategoryChanges.All(x => x.Category.Id != category.Id))
                Sl.CategoryChangeRepo.AddUpdateEntry(category, memuchoUser, imageWasUpdated:false);
        }
    }
}
