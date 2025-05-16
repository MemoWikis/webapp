public class SideSheetCreateWikiModalController
(
    SessionUser _sessionUser,
    WikiCreator _wikiCreator,
    UserWritingRepo _userWritingRepo) : ApiBaseController
{
    [HttpPost]
    public CreateWikiResponse CreateWiki([FromBody] CreateWikiRequest createWikiRequest)
    {
        if (string.IsNullOrEmpty(createWikiRequest.Name))
            throw new ArgumentNullException(nameof(createWikiRequest), "Invalid request");

        var createWikiResult = _wikiCreator.Create(createWikiRequest.Name, _sessionUser);

        if (!createWikiResult.Success)
            return new CreateWikiResponse(Success: false, createWikiResult.MessageKey);

        var newWiki = createWikiResult.TinyWikiItem;

        var userCacheItem = EntityCache.GetUserById(_sessionUser.UserId);

        _userWritingRepo.Update(userCacheItem);

        return new CreateWikiResponse(true, null, new CreateWikiData(newWiki.Id, newWiki.Name));
    }

    public readonly record struct CreateWikiRequest(string Name);

    public readonly record struct CreateWikiResponse(bool Success, string? MessageKey = null, CreateWikiData? Data = null);

    public readonly record struct CreateWikiData(int Id, string Name);
}
