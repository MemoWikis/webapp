using System.Collections.Generic;
using System.Linq;

namespace TrueOrFalse.Search; 
public class GlobalSearchResult
{
    public ISearchCategoriesResult CategoriesResult;
    private IList<CategoryCacheItem> _categories;
    public IList<CategoryCacheItem> Categories => _categories ??= CategoriesResult.GetCategories();
    public int CategoriesResultCount => CategoriesResult.Count;
    public ISearchQuestionsResult QuestionsResult;
    private IList<QuestionCacheItem> _questions;
    public IList<QuestionCacheItem> Questions => _questions ??= QuestionsResult.GetQuestions();
    public int QuestionsResultCount => QuestionsResult.Count;

    public ISearchUsersResult UsersResult;
    private IList<UserCacheItem> _users;
    public IList<UserCacheItem> Users => _users ??= UsersResult.GetUsers();
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
                _categories = Categories.Take(nine).ToList();
        } else if (type == "Questions") {
            if (TotalElements >= maxElements)
                _questions = Questions.Take(nine).ToList();
        }

        if (TotalElements >= maxElements)
            _users = Users.Take(none).ToList();
    }
}