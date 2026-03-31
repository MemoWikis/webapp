public class WikisController(
    WikiListService _wikiListService) : ApiBaseController
{
    [HttpGet]
    public WikiListService.WikisResult Get(int page = 1, int pageSize = 20)
    {
        return _wikiListService.GetPublicWikis(page, pageSize);
    }
}
