namespace TrueOrFalse.Search;

public class GlobalSearchResult
{
    public ISearchPagesResult PagesResult;
    private IList<PageCacheItem> _pages;
    public IList<PageCacheItem> Pages => _pages ??= PagesResult.GetPages();
    public int PageCount => PagesResult.Count;
    public ISearchQuestionsResult QuestionsResult;
    private IList<QuestionCacheItem> _questions;
    public IList<QuestionCacheItem> Questions => _questions ??= QuestionsResult.GetQuestions();
    public int QuestionsResultCount => QuestionsResult.Count;

    public ISearchUsersResult UsersResult;
    private IList<UserCacheItem> _users;
    public IList<UserCacheItem> Users => _users ??= UsersResult.GetUsers();
    public int UsersResultCount => UsersResult.Count;

    public int TotalElements => Pages.Count + Questions.Count + Users.Count;
}