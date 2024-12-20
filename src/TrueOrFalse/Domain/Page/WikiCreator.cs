﻿public class WikiCreator(
    Logg _logg,
    PageRepository _pageRepository,
    UserReadingRepo _userReadingRepo) : IRegisterAsInstancePerLifetime
{

    public readonly record struct CreateResult(
        bool Success,
        string? MessageKey,
        TinyWikiItem? TinyWikiItem = null);

    public readonly record struct TinyWikiItem(string Name, int Id);

    public CreateResult Create(string name, SessionUser sessionUser)
    {
        if (!new LimitCheck(_logg, sessionUser).CanSavePrivatePage(true))
        {
            return new CreateResult(
                Success: false,
                MessageKey: FrontendMessageKeys.Error.Subscription.CantSavePrivatePage
            );
        }

        var wiki = new Page(name, sessionUser.UserId);

        wiki.Creator = _userReadingRepo.GetById(sessionUser.UserId);
        wiki.Type = PageType.Standard;
        wiki.Visibility = PageVisibility.Owner;
        wiki.IsWiki = true;
        _pageRepository.Create(wiki);

        return new CreateResult(
            Success: true,
            MessageKey: null,
            TinyWikiItem: new TinyWikiItem(
                Name: wiki.Name,
                Id: wiki.Id
            )
        );
    }
}