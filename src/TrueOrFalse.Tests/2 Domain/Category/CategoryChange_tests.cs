using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;
using NUnit.Framework;
using TrueOrFalse;
using TrueOrFalse.Tests;

[TestFixture]
class CategoryChange_tests : BaseTest
{
    private CategoryChangeDayModel _currentCategoryChangeDayModel;
    private CategoryChangeDetailModel _currentCategoryChangeDetailModel;


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

        _currentCategoryChangeDayModel = GetCategoryChangeDayModel(categoryChanges.OrderByDescending(cc => cc.DateCreated).ToList());
        var mergedList = _currentCategoryChangeDayModel.Items;

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


        _currentCategoryChangeDayModel =
            GetCategoryChangeDayModel(categoryChanges.OrderByDescending(cc => cc.DateCreated).ToList());
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

        for (int i = 0; i < changes.Count; i++)
            GetMergedItems(changes[i], items);

        _currentCategoryChangeDayModel.Date = DateTime.Now.ToString("dd.MM.yyyy");
        _currentCategoryChangeDayModel.DateTime = DateTime.Now;
        _currentCategoryChangeDayModel.Items = items;

        return _currentCategoryChangeDayModel;
    }

    public void GetMergedItems(CategoryChange change, List<CategoryChangeDetailModel> items)
    {
        if (change.Category == null || change.Category.IsNotVisibleToCurrentUser)
            return;
        if (_currentCategoryChangeDetailModel != null &&
            change.Author.Id == _currentCategoryChangeDetailModel.Author.Id &&
            change.Category.Visibility == _currentCategoryChangeDetailModel.Visibility &&
            change.Type == CategoryChangeType.Text &&
            _currentCategoryChangeDetailModel.Type == change.Type &&
            (_currentCategoryChangeDetailModel.AggregatedCategoryChangeDetailModel.Last().DateCreated - change.DateCreated).TotalMinutes < 15)
            _currentCategoryChangeDetailModel.AggregatedCategoryChangeDetailModel.Add(_currentCategoryChangeDayModel.GetCategoryChangeDetailModel(change));
        else
        {
            var newDetailModel = _currentCategoryChangeDayModel.GetCategoryChangeDetailModel(change);
            newDetailModel.AggregatedCategoryChangeDetailModel = new List<CategoryChangeDetailModel> { newDetailModel };
            _currentCategoryChangeDetailModel = newDetailModel;
            items.Add(newDetailModel);
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