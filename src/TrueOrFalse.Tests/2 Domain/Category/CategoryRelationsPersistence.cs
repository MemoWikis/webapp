using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using NHibernate.Criterion;
using NUnit.Framework;
using TrueOrFalse;
using TrueOrFalse.Tests;
using TrueOrFalse.Tests._2_Domain.Question.LearnSession;

class CategoryRelationsPersistence : BaseTest
{
    [Test]
    public void ShouldAggregateGrandChildren()
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

        QuestionsRelationsCheck(editCategoryController, user);

        Assert.That(firstLevelCategory.CategoryRelations.Count, Is.EqualTo(3));
        Assert.That(secondLevelCategory.CategoryRelations.Count, Is.EqualTo(2));
        Assert.That(thirdLevelCategory.CategoryRelations.Count, Is.EqualTo(1));
        Assert.That(fourthLevelCategory.CategoryRelations.Count, Is.EqualTo(0));

        Assert.That(firstLevelCategory.CategoryRelations.Count, Is.EqualTo(0));
        Assert.That(secondLevelCategory.CategoryRelations.Count, Is.EqualTo(1));
        Assert.That(thirdLevelCategory.CategoryRelations.Count, Is.EqualTo(1));
        Assert.That(fourthLevelCategory.CategoryRelations.Count, Is.EqualTo(1));
    }

    public void QuestionsRelationsCheck(EditCategoryController editCategoryController, User user)
    {
        var level4Category = Sl.CategoryRepo.GetByName("Level4").FirstOrDefault();
        var questionCategories = new List<Category>();
        questionCategories.Add(level4Category);
        var question = new Question()
        {
            Categories = questionCategories,
            Id = 1,
            DescriptionHtml = "HUHU",
            SolutionType = SolutionType.FlashCard,
            Visibility = 0,
            Creator = user,
            TextHtml = "TextHtml",
            DateCreated = DateTime.Now,
            Description = "Description",
            Text = "Text"
        };
        Sl.QuestionChangeRepo.AddUpdateEntry(question);

        var level1Category = new Category("Level1_1");
        level1Category.Creator = user;
        Sl.CategoryRepo.Create(level1Category);
        level4Category.UpdateCountQuestionsAggregated();
        level1Category.UpdateCountQuestionsAggregated();
        Assert.That(level4Category.CountQuestionsAggregated, Is.EqualTo(1));
        Assert.That(level1Category.CountQuestionsAggregated, Is.EqualTo(0));
    }
}