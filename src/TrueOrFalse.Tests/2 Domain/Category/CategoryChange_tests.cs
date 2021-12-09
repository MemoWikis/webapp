using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;
using NUnit.Framework;
using TrueOrFalse.Tests;

[TestFixture]
class CategoryChange_tests : BaseTest
{
    private CategoryChangeDayModel _currentCategoryChangeDayModel;

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
        var currentRev = new CategoryChange
        {
            DateCreated = new DateTime(2021, 12, 5, 11, 0, 0),
            Category = category,
            Author = user,
            Type = CategoryChangeType.Text,
            Id = 7
        };

        var relationToMerge2 = new CategoryChange
        {
            DateCreated = new DateTime(2021, 12, 5, 10, 30, 0),
            Category = category,
            Author = user,
            Type = CategoryChangeType.Relations,
            Id = 6
        };

        var textToMerge3 = new CategoryChange
        {
            DateCreated = new DateTime(2021, 12, 5, 10, 20, 0),
            Category = category,
            Author = user,
            Type = CategoryChangeType.Text,
            Id = 5
        };
        var relationToMerge1 = new CategoryChange
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
        categoryChanges.Add(relationToMerge1);
        categoryChanges.Add(textToMerge3);
        categoryChanges.Add(relationToMerge2);
        categoryChanges.Add(currentRev);

        _currentCategoryChangeDayModel = GetCategoryChangeDayModel(categoryChanges);
        var mergedList = _currentCategoryChangeDayModel.Items;

        Assert.That(mergedList.Count, Is.EqualTo(3));
    }

    [Test]
    public void Should_merge_simple_category_changes()
    {
        var category = ContextCategory.New().Add("Category").Persist().All[0];
        var user = ContextUser.New().Add("user").Persist().All[0];
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

        _currentCategoryChangeDayModel = GetCategoryChangeDayModel(categoryChanges);
        var mergedList = _currentCategoryChangeDayModel.Items;

        Assert.That(mergedList.Count, Is.EqualTo(3));
    }

    //public List<CategoryChangeDetailModel> GetMergedItems(IList<CategoryChange> changes)
    //{
    //    var unsortedList = new List<CategoryChangeDetailModel>();
    //    var groupedList = changes.OrderBy(cc => cc.DateCreated).GroupBy(cc => cc.Type);

    //    foreach (var group in groupedList)
    //    {
    //        CategoryChange previousChange = null;
    //        CategoryChangeViewItem currentCategoryChangeViewItem = null;
    //        var i = 1;

    //        var sortedChanges = group.OrderBy(cc => cc.DateCreated);
    //        foreach (var cc in sortedChanges)
    //        {
    //            if (currentCategoryChangeViewItem != null && previousChange != null)
    //            {
    //                if ((cc.DateCreated - previousChange.DateCreated).TotalMinutes <= 15 
    //                    && cc.Category.Visibility == previousChange.Category.Visibility
    //                    && cc.Author == previousChange.Author)
    //                {
    //                    currentCategoryChangeViewItem.LastEdit = cc.DateCreated;
    //                    currentCategoryChangeViewItem.CategoryChanges.Add(cc);
    //                    if (group.Count() == i)
    //                        unsortedList.Add(new CategoryChangeDetailModel(cc));
    //                }
    //                else
    //                {
    //                    unsortedList.Add(currentCategoryChangeViewItem);
    //                    currentCategoryChangeViewItem = GetCategoryChangeSession(cc);
    //                    currentCategoryChangeViewItem.CategoryChanges.Add(cc);
    //                    if (group.Count() == i)
    //                        unsortedList.Add(currentCategoryChangeViewItem);
    //                }
    //            }
    //            else
    //            {   
    //                currentCategoryChangeViewItem = GetCategoryChangeSession(cc);
    //                currentCategoryChangeViewItem.CategoryChanges.Add(cc);
    //                unsortedList.Add(currentCategoryChangeViewItem);
    //            }

    //            previousChange = cc;
    //            i++;
    //        }
    //    }

    //    return unsortedList.OrderByDescending(cc => cc.LastEdit).ToList();
    //}

    public CategoryChangeDayModel GetCategoryChangeDayModel(IList<CategoryChange> changes)
    {
        _currentCategoryChangeDayModel = new CategoryChangeDayModel(DateTime.Now, changes);
        var items = new List<CategoryChangeDetailModel>();
        var currenCategoryChangeDetailModel = new CategoryChangeDetailModel();

        changes
            .Where(cc => cc.Category != null && cc.Category.IsVisibleToCurrentUser()).ToList().ForEach(cc => GetMergedItems(cc, currenCategoryChangeDetailModel, items));

        _currentCategoryChangeDayModel.Date = DateTime.Now.ToString("dd.MM.yyyy");
        _currentCategoryChangeDayModel.DateTime = DateTime.Now;
        _currentCategoryChangeDayModel.Items = items;

        return _currentCategoryChangeDayModel;
    }

    public void GetMergedItems(CategoryChange change, CategoryChangeDetailModel currenCategoryChangeDetailModel, List<CategoryChangeDetailModel> items)
    {
        if (change.Author.Id == currenCategoryChangeDetailModel.Author.Id &&
            change.Category.Visibility == currenCategoryChangeDetailModel.Visibility &&
            change.Type == CategoryChangeType.Text &&
            (currenCategoryChangeDetailModel.AggregatedCategoryChangeDetailModel.Last().DateCreated - change.DateCreated).TotalMinutes < 15)
            currenCategoryChangeDetailModel.AggregatedCategoryChangeDetailModel.Add(_currentCategoryChangeDayModel.GetCategoryChangeDetailModel(change));
        else
        {
            var newDetailModel = _currentCategoryChangeDayModel.GetCategoryChangeDetailModel(change);
            items.Add(newDetailModel);
            currenCategoryChangeDetailModel = newDetailModel;
        }
    }

    public CategoryChangeViewItem GetCategoryChangeSession(CategoryChange cc)
    {
        var categoryChangeSession = new CategoryChangeViewItem
        {
            Author = new UserTinyModel(cc.Author),
            LastEdit = cc.DateCreated,
            FirstEdit = cc.DateCreated,
            Visibility = cc.Category.Visibility,
            Type = cc.Type,
            CategoryChanges = new List<CategoryChange>()
        };

        return categoryChangeSession;
    }
}