public class PageCreator(
    Logg _logg,
    PageRepository _pageRepository,
    UserReadingRepo _userReadingRepo,
    PageRelationRepo _pageRelationRepo,
    UserWritingRepo _userWritingRepo)
    : IRegisterAsInstancePerLifetime
{
    public readonly record struct CreateResult(
        bool Success,
        string MessageKey,
        TinyPageItem Data);

    public readonly record struct TinyPageItem(bool CantSavePrivatePage, string Name, int Id);

    public CreateResult Create(string name, int parentPageId, SessionUser sessionUser)
    {
        if (!new LimitCheck(_logg, sessionUser).CanSavePrivatePage(true))
        {
            return new CreateResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Subscription.CantSavePrivatePage,
                Data = new TinyPageItem
                {
                    CantSavePrivatePage = true
                }
            };
        }

        var page = new Page(name, sessionUser.UserId);

        page.Creator = _userReadingRepo.GetById(sessionUser.UserId);
        page.Type = PageType.Standard;
        page.Visibility = PageVisibility.Owner;
        page.Language = sessionUser.User.UiLanguage;
        _pageRepository.Create(page);

        var user = EntityCache.GetUserById(sessionUser.UserId);
        LanguageExtensions.AddContentLanguageToUser(user, page.Language);
        _userWritingRepo.Update(user);

        var modifyRelationsForPage = new ModifyRelationsForPage(_pageRepository, _pageRelationRepo);
        modifyRelationsForPage.AddChild(parentPageId, page.Id, sessionUser.UserId);

        return new CreateResult
        {
            Success = true,
            Data = new TinyPageItem
            {
                Name = page.Name,
                Id = page.Id
            }
        };
    }
}