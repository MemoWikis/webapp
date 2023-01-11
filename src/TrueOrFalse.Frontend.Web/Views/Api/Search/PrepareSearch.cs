﻿using System.Collections.Generic;
using System.Linq;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;

public class PrepareSearch
{
    public static SearchBoxElements Go(string term, string type)
    {
        var result = new SearchBoxElements();

        var pageSize = 5;
        result.CategoriesResult = Sl.SearchCategories.Run(term, new Pager { PageSize = pageSize });

        result.UsersResult = Sl.SearchUsers.Run(term, new Pager { PageSize = pageSize }, SearchUsersOrderBy.None);

        var searchSpec = Sl.SessionUiData.SearchSpecQuestionSearchBox;
        searchSpec.OrderBy.BestMatch.Desc();
        searchSpec.Filter.SearchTerm = term;
        searchSpec.Filter.IgnorePrivates = true;
        searchSpec.PageSize = pageSize;

        result.QuestionsResult = Sl.SearchQuestions.Run(term, searchSpec);

        if (type != null)
            result.Ensure_max_elements_per_type_count_of_9("Categories");
        else
            result.Ensure_max_element_count_of_12();

        return result;
    }

    public static SearchBoxElements GoAllCategories(string term, int[] categoriesToFilter = null)
    {
        var pager = new Pager {QueryAll = true};

        var result = new SearchBoxElements
        {
            CategoriesResult = Sl.SearchCategories.Run(term, pager, categoriesToFilter: categoriesToFilter)
        };
        return result;
    }
}

public class SearchBoxElements
{
    public SearchCategoriesResult CategoriesResult;
    private IList<Category> _categories;
    public IList<Category> Categories => _categories ?? (_categories = CategoriesResult.GetCategories());
    public int CategoriesResultCount => CategoriesResult.Count;
    public SearchQuestionsResult QuestionsResult;
    private IList<Question> _questions;
    public IList<Question> Questions => _questions ?? (_questions = QuestionsResult.GetQuestions());
    public int QuestionsResultCount => QuestionsResult.Count;

    public SearchUsersResult UsersResult;
    private IList<User> _users;
    public IList<User> Users => _users ?? (_users = UsersResult.GetUsers());
    public int UsersResultCount => UsersResult.Count;

    public int TotalElements  =>  Categories.Count + Questions.Count + Users.Count;

    public void Ensure_max_element_count_of_12()
    {
        var maxElements = 9;

        //first round
        var three = 3;
        if (TotalElements >= maxElements)
            _users = Users.Take(three).ToList();

        if (TotalElements >= maxElements)
            _questions = Questions.Take(three).ToList();

        if (TotalElements >= maxElements)
            _categories = Categories.Take(three).ToList();

        //second round
        var two = 2;

        if (TotalElements >= maxElements)
            _questions = Questions.Take(two).ToList();
    }

    public void Ensure_max_elements_per_type_count_of_9(string type)
    {
        var maxElements = 9;
        var nine = 9;
        var none = 0;

        if (type == "Categories")
        {
            if (TotalElements >= maxElements)
                _categories = Categories.Take(9).ToList();
        } else if (type == "Questions") {
            if (TotalElements >= maxElements)
                _questions = Questions.Take(nine).ToList();
        }

        if (TotalElements >= maxElements)
            _users = Users.Take(none).ToList();
    }
}