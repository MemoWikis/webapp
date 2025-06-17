public class MiddlewareStartpageController(SessionUser _sessionUser) : ApiBaseController
{
    public readonly record struct TinyPage(int Id, string Name);

    [HttpGet]
    public TinyPage Get()
    {
        var page = _sessionUser.IsLoggedIn
            ? _sessionUser.User.FirstWiki()
            : FeaturedPage.GetRootPage;
        
        return new TinyPage { Name = page.Name, Id = page.Id };
    }
}