using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Util;
using NUnit.Framework;
using TrueOrFalse;
using TrueOrFalse.Tests;

[TestFixture]
class CategoryChange_tests : BaseTest
{
    [Test]
    public void Should_save_category_changes()
    {
        var category = ContextCategory.New().Add("Category 1").Persist().All[0];
        category.Name = "Category 2";

        Sl.CategoryRepo.Update(category);

        Assert.That(Sl.CategoryRepo.GetAllEager().ToList().First().Name, Is.EqualTo("Category 2"));
    }

    [Test]
    public void Should_merge_complex_category_changes()
    {
        var category = ContextCategory.New().Add("Category").Persist().All[0];
        var user = ContextUser.New().Add("user").Persist().All[0];
        user.EmailAddress = "testmemucho@memucho.memucho";
        Sl.UserRepo.Update(user);
        var currentRev = new CategoryChange
        {
            DateCreated = new DateTime(2021, 12, 5, 11, 0, 0),
            Category = category,
            Author = user,
            Type = CategoryChangeType.Text,
            Id = 7
        };

        var relation2 = new CategoryChange
        {
            DateCreated = new DateTime(2021, 12, 5, 10, 30, 0),
            Category = category,
            Author = user,
            Type = CategoryChangeType.Relations,
            Id = 6
        };

        var text3 = new CategoryChange
        {
            DateCreated = new DateTime(2021, 12, 5, 10, 20, 0),
            Category = category,
            Author = user,
            Type = CategoryChangeType.Text,
            Id = 5
        };
        var relation1 = new CategoryChange
        {
            DateCreated = new DateTime(2021, 12, 5, 10, 17, 0),
            Category = category,
            Author = user,
            Type = CategoryChangeType.Relations,
            Id = 4
        };
        var textToMerge2 = new CategoryChange
        {
            DateCreated = new DateTime(2021, 12, 5, 10, 13, 0),
            Category = category,
            Author = user,
            Type = CategoryChangeType.Text,
            Id = 3
        };
        var textToMerge1 = new CategoryChange
        {
            DateCreated = new DateTime(2021, 12, 5, 10, 0, 0),
            Category = category,
            Author = user,
            Type = CategoryChangeType.Text,
            Id = 2
        };
        var initialRev = new CategoryChange
        {
            DateCreated = new DateTime(2021, 12, 1, 1, 0, 0),
            Category = category,
            Author = user,
            Type = CategoryChangeType.Create,
            Id = 1
        };

        var categoryChanges = new List<CategoryChange>();
        categoryChanges.Add(initialRev);
        categoryChanges.Add(textToMerge1);
        categoryChanges.Add(textToMerge2);
        categoryChanges.Add(relation1);
        categoryChanges.Add(text3);
        categoryChanges.Add(relation2);
        categoryChanges.Add(currentRev);

        var categoryChangeDayModel = new CategoryChangeDayModel(DateTime.Now,
            categoryChanges.OrderByDescending(cc => cc.DateCreated).ToList());

        var mergedList = categoryChangeDayModel.Items;

        Assert.That(mergedList.Count, Is.EqualTo(6));
    }

    [Test]
    public void Should_merge_simple_category_changes()
    {
        var category = ContextCategory.New().Add("Category").Persist().All[0];
        var user = ContextUser.New().Add("user").Persist().All[0];
        user.EmailAddress = "testmemucho@memucho.memucho";
        Sl.UserRepo.Update(user);
        var currentRev = new CategoryChange
        {
            DateCreated = new DateTime(2021, 12, 5, 11, 0, 0),
            Category = category,
            Author = user,
            Type = CategoryChangeType.Text,
            Id = 5
        };
        var revToMerge3 = new CategoryChange
        {
            DateCreated = new DateTime(2021, 12, 5, 10, 20, 0),
            Category = category,
            Author = user,
            Type = CategoryChangeType.Text,
            Id = 4
        };
        var revToMerge2 = new CategoryChange
        {
            DateCreated = new DateTime(2021, 12, 5, 10, 13, 0),
            Category = category,
            Author = user,
            Type = CategoryChangeType.Text,
            Id = 3
        };
        var revToMerge1 = new CategoryChange
        {
            DateCreated = new DateTime(2021, 12, 5, 10, 0, 0),
            Category = category,
            Author = user,
            Type = CategoryChangeType.Text,
            Id = 2
        };
        var initialRev = new CategoryChange
        {
            DateCreated = new DateTime(2021, 12, 1, 1, 0, 0),
            Category = category,
            Author = user,
            Type = CategoryChangeType.Create,
            Id = 1
        };

        var categoryChanges = new List<CategoryChange>();
        categoryChanges.Add(initialRev);
        categoryChanges.Add(revToMerge1);
        categoryChanges.Add(revToMerge2);
        categoryChanges.Add(revToMerge3);
        categoryChanges.Add(currentRev);

        var categoryChangeDayModel = new CategoryChangeDayModel(DateTime.Now,
            categoryChanges.OrderByDescending(cc => cc.DateCreated).ToList());

        var mergedList = categoryChangeDayModel.Items;

        Assert.That(mergedList.Count, Is.EqualTo(3));
    }

    [Test]
    public void XmlTest()
    {
        var brokenString =
            "&amp;";

        var model = new CategoryHistoryDetailModel(true);
        var formatted = model.FormatHtmlString(brokenString);

        var empty = model.FormatHtmlString(null);

        Assert.That(formatted, Is.EqualTo("&"));
        Assert.That(empty, Is.EqualTo(""));
    }

    [Test]
    public void SafeImgXmlTest()
    {
        var imgString =
            "<img src=\"data:image/png;base64,YII=\" alt=\"0\">";

        var model = new CategoryHistoryDetailModel(true);
        var formatted = model.FormatHtmlString(imgString);

        Assert.That(formatted, Is.EqualTo("\r\n  <img src=\"data:image/png;base64,YII=\" alt=\"0\">\r\n "));
    }

    [Test]
    public void GetNullRelationChangeItemForNonExistingPreviousRevision()
    {
        var category = ContextCategory.New().Add("Category 1").Persist().All[0];
        var changes = new List<CategoryChange>();

        changes.Add(
            new CategoryChange
                {
                    Id = 1,
                    Author = new User(),
                    Category = category,
                    Data = "{\"CategoryRelations\":[],\"ImageWasUpdated\":false,\"Name\":\"M Ts Wiki\",\"Description\":null,\"TopicMarkdown\":null,\"Content\":\"\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":1}",
                    DataVersion = 1,
                    DateCreated = new DateTime(2010, 10, 10, 10, 10, 10),
                    ShowInSidebar = false,
                    Type = CategoryChangeType.Create
                }
            );

        var item = new CategoryChangeDetailModel();
        item.CategoryChangeId = 1;
        var relationChangeItem = RelationChangeItem.GetRelationChangeItem(item, changes);

        Assert.That(relationChangeItem, Is.EqualTo(null));
    }
}