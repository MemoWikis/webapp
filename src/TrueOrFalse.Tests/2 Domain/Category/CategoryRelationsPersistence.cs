using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using NHibernate.Criterion;
using NUnit.Framework;
using TrueOrFalse.Tests;
using TrueOrFalse.Tests._2_Domain.Question.LearnSession;

class CategoryRelationsPersistence : BaseTest
{
    [Test]
    public void RelationsCheckTest()
    {
        var editCategoryController = new EditCategoryController(Sl.CategoryRepo);
        var user = ContextUser.GetUser();
        var rootCategory = new Category("Root");
        rootCategory.Creator = user;
        Sl.CategoryRepo.Create(rootCategory);
        var firstLevelCategory = new Category("Level1");
        firstLevelCategory.Creator = user;
        Sl.CategoryRepo.Create(firstLevelCategory);
        var secondLevelCategory = new Category("Level2"); 
        secondLevelCategory.Creator = user;
        secondLevelCategory.ParentCategories().Add(firstLevelCategory);
        Sl.CategoryRepo.Create(secondLevelCategory);
        editCategoryController.AddChild(secondLevelCategory.Id, firstLevelCategory.Id);
        var thirdLevelCategory = new Category("Level3");
        thirdLevelCategory.Creator = user;
        thirdLevelCategory.ParentCategories().Add(secondLevelCategory);
        Sl.CategoryRepo.Create(thirdLevelCategory);
        editCategoryController.AddChild(thirdLevelCategory.Id, secondLevelCategory.Id);
        var fourthLevelCategory = new Category("Level4");
        fourthLevelCategory.Creator = user;
        fourthLevelCategory.ParentCategories().Add(thirdLevelCategory);
        Sl.CategoryRepo.Create(fourthLevelCategory);
        editCategoryController.AddChild(fourthLevelCategory.Id, thirdLevelCategory.Id);

        Assert.That(firstLevelCategory.CategoryRelations.Count,Is.EqualTo(3));
    }
}