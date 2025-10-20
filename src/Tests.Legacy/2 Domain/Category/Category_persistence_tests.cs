using System.Collections.Generic;
using System.Linq;
using Autofac;
using NHibernate.Util;
using NUnit.Framework;

namespace TrueOrFalse.Tests.Persistence;

[Category(TestCategories.Programmer)]
public class Category_persistence_tests : BaseTest
{
    [Test]
    public void Category_should_be_persisted()
    {
        var user = new User { Name = "Some user" };
        R<UserWritingRepo>().Create(user);
        var sessionUser = R<SessionUser>(); 
        var categoryRepo = R<CategoryRepository>();

       

        var category = new Category("Sports",sessionUser.UserId)
        {
            Creator = user,
            Type = CategoryType.Standard
        };
        categoryRepo.Create(category);


        var categoryFromDb = categoryRepo.GetAll().First();
        Assert.That(categoryFromDb.Name, Is.EqualTo("Sports"));
        Assert.That(categoryFromDb.Type, Is.EqualTo(CategoryType.Standard));

        RecycleContainer();

        categoryRepo = Resolve<CategoryRepository>();
        categoryFromDb = categoryRepo.GetAll().First();
        var categoryFromDb2 = categoryRepo.GetById(categoryFromDb.Id);
        Assert.That(categoryFromDb2.Type, Is.EqualTo(CategoryType.Standard));
    }

    [Test]
    public void Category_should_load_children_of_type()
    {
        var context = ContextCategory.New()
            .Add("Daily-A", CategoryType.Daily)
            .Add("Daily-B", CategoryType.Daily)
            .Add("DailyIssue-1", CategoryType.DailyIssue)
            .Add("DailyIssue-2", CategoryType.DailyIssue)
            .Add("DailyIssue-3", CategoryType.DailyIssue)
            .Add("Standard-1", CategoryType.Standard)
            .Persist();

        context.All
            .Where(c => c.Name.StartsWith("DailyIssue"))
            .ForEach(c =>
            {
                var parentCategories = new List<Category>();
                parentCategories.Add(context.All.First(x => x.Name.StartsWith("Daily-A")));
                parentCategories.Add(context.All.First(x => x.Name.StartsWith("Standard-1")));

                new ModifyRelationsForCategory(LifetimeScope.Resolve<CategoryRepository>()).UpdateCategoryRelationsOfType(c.Id,
                    parentCategories.Select(cat => cat.Id).ToList());
            });

        context.Update();

        var children = R<CategoryRepository>().GetChildren(
            CategoryType.Daily, CategoryType.DailyIssue, context.All.First(x => x.Name == "Daily-A").Id);

        Assert.That(children.Count, Is.EqualTo(3));
        Assert.That(children.Any(c => c.Name == "DailyIssue-1"), Is.True);
        Assert.That(children.Any(c => c.Name == "DailyIssue-2"), Is.True);
        Assert.That(children.Any(c => c.Name == "DailyIssue-3"), Is.True);
    }

    [Test]
    public void Get_correct_aggregated_categories()
    {
        var context = ContextCategory.New();
        var user = context.AddCaseThreeToCache();
        var permissionCheck = Resolve<PermissionCheck>(); 

        var categories = EntityCache.GetAllCategories();

        for (int i = 0; i < categories.Count; i++)
        {
            if (categories[i].Name != "A")
            {
                var category = categories.ByName("A");

                category.CategoryRelations.Add(
                    new CategoryCacheRelation()
                    {
                        ChildCategoryId = category.Id,
                        RelatedCategoryId = categories[i].Id
                    });
                EntityCache.AddOrUpdate(category);
                context.Update(LifetimeScope.Resolve<CategoryRepository>().GetByIdEager(category.Id));
            }
        }
        categories = EntityCache.GetAllCategories();

        var expectedResult = EntityCache.GetCategoryByName("A").First()
            .AggregatedCategories(permissionCheck)
            .Count(cci => SessionUserCache.IsInWishKnowledge(user.Id, cci.Key, Resolve<CategoryValuationReadingRepo>(), R<UserReadingRepo>(),
                R<QuestionValuationRepo>())); 

        Assert.That(expectedResult, Is.EqualTo(6));
        Assert.That(categories.ByName("A").AggregatedCategories(permissionCheck, true).Count, Is.EqualTo(13));
    }
}