using System.Linq;

public class WuwiMigrator
{
    private static CategoryRepository _categoryRepo = Sl.CategoryRepo;
    private static CategoryValuationRepo _categoryValuationRepo = Sl.CategoryValuationRepo;

    public static void CreateWuwiCategoryForAllUsers()
    {
        var users = Sl.UserRepo.GetAll();

        foreach (var user in users)
        {
            var wuwiCategories = Sl.CategoryValuationRepo.GetByUser(user.Id).Where(cv => cv.IsInWishKnowledge()).ToList();
            if (wuwiCategories.Any())
            {
                Logg.r().Information("WuwiMigration - Start Migration for User: {userId}", user.Id);

                var wuwiCategory = CreateWuwiCategory(user);

                foreach (var cv in wuwiCategories)
                    AddCategoryToWuwiCategory(wuwiCategory, cv, user);

                Logg.r().Information("WuwiMigration - End Migration for User: {userId}", user.Id);
            }
        }
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

        Sl.CategoryRepo.CreateOnlyDb(category);

        ModifyRelationsForCategory.AddParentCategory(category, user.StartTopicId);

        return category;
    }

    public static void AddCategoryToWuwiCategory(Category wuwiCategory, CategoryValuation categoryValuation, User user)
    {
        var childCategory = _categoryRepo.GetById(categoryValuation.CategoryId);

        ModifyRelationsForCategory.AddParentCategory(childCategory, wuwiCategory.Id);

        _categoryValuationRepo.Delete(categoryValuation);

        Logg.r().Information("WuwiMigration - Migrated Child: {childId}, to WuwiCategory: {parentId}", 
            categoryValuation.CategoryId,
            wuwiCategory.Id);
    }

    public static string PersonalWuwiContent = "<h2></h2>" +
                                               "<p><strong>dies ist deine persönliche Startseite!</strong> Du kannst diesen Text leicht ändern, indem du einfach hier anfängst zu tippen. Probier mal!</p>" +
                                               "<p>Wenn du Fragen hast, melde dich. Wir helfen dir gerne! :-)</p> " +
                                               "<p><strong>Liebe Grüße, dein memucho-Team.</strong></p>";
}