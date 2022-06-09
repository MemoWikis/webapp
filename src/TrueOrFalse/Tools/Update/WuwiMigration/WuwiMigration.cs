using System.Collections.Generic;
using System.Linq;

public class WuwiMigrator
{
    private static CategoryRepository _categoryRepository = Sl.CategoryRepo;
    private static IList<Category> _allCategories = _categoryRepository.GetAll();
    private static CategoryValuationRepo _categoryValuationRepo = Sl.CategoryValuationRepo;

    public static void CreateWuwiCategoryForAllUsers()
    {
        var users = Sl.UserRepo.GetAll();
        var allCategoryValuations = _categoryValuationRepo.GetAll();

        foreach (var user in users)
        {
            var wuwiCategories = allCategoryValuations.Where(cv => cv.UserId == user.Id && cv.IsInWishKnowledge()).ToList();
            if (wuwiCategories.Any())
            {
                Logg.r().Information("WuwiMigration - Start Migration for User: {userId}", user.Id);

                var wuwiCategory = CreateWuwiCategory(user);

                foreach (var cv in wuwiCategories)
                    AddCategoryToWuwiCategory(wuwiCategory, cv);

                Logg.r().Information("WuwiMigration - End Migration for User: {userId}", user.Id);
            }
        }
        _categoryValuationRepo.Flush();
    }

    public static Category CreateWuwiCategory(User user)
    {
        var category = new Category
        {
            Name = user.Name + "s Wunschwissen",
            Content = PersonalWuwiContent,
            Visibility = CategoryVisibility.Owner,
            Creator = user,
            Type = CategoryType.Standard,
            IsUserStartTopic = false
        };

        _categoryRepository.CreateOnlyDb(category);

        ModifyRelationsForCategory.AddParentCategory(category, user.StartTopicId);

        return category;
    }

    public static void AddCategoryToWuwiCategory(Category wuwiCategory, CategoryValuation categoryValuation)
    {
        var childCategory = _allCategories.FirstOrDefault(c => c.Id == categoryValuation.CategoryId);

        ModifyRelationsForCategory.AddParentCategory(childCategory, wuwiCategory.Id);

        _categoryValuationRepo.DeleteWithoutFlush(categoryValuation);

        Logg.r().Information("WuwiMigration - Migrated Child: {childId}, to WuwiCategory: {parentId}", 
            categoryValuation.CategoryId,
            wuwiCategory.Id);
    }

    public static string PersonalWuwiContent = "<p><strong>Dein Wunschwissen ab jetzt hier:</strong></p>" +
                                               "<p>Unsere Wunschwissen-Funktion ist ab sofort nur noch für Fragen verfügbar. Deine bisherigen Wunschwissen-Themen haben wir hier gesichert.</p> ";
}