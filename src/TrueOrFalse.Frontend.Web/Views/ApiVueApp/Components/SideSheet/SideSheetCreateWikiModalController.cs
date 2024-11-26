using Microsoft.AspNetCore.Mvc;
using System;

namespace VueApp;

public class SideSheetCreateWikiModalController
(
    SessionUser _sessionUser,
    WikiCreator _wikiCreator) : Controller
{
    [HttpPost]
    public CreateWikiResponse CreateWiki([FromBody] CreateWikiRequest req)
    {
        if (req == null || string.IsNullOrEmpty(req.Name))
        {
            throw new ArgumentNullException(nameof(req), "Invalid request");
        }

        var createWikiResult = _wikiCreator.Create(req.Name, _sessionUser);

        if (!createWikiResult.Success)
        {
            return new CreateWikiResponse(false, createWikiResult.MessageKey);
        }

        if (createWikiResult.TinyWikiItem == null)
        {
            return new CreateWikiResponse(false, createWikiResult.MessageKey);
        }

        var newWiki = createWikiResult.TinyWikiItem ?? throw new Exception("Failed to create wiki.");

        var userCacheItem = EntityCache.GetUserById(_sessionUser.UserId);
        userCacheItem.AddWiki(newWiki.Id);

        return new CreateWikiResponse(true, null, new CreateWikiData(newWiki.Id, newWiki.Name));
    }

    public readonly record struct CreateWikiRequest(string Name);
    public readonly record struct CreateWikiResponse(bool Success, string? MessageKey = null, CreateWikiData? Data = null);
    public readonly record struct CreateWikiData(int Id, string Name);
}
