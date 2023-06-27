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

}