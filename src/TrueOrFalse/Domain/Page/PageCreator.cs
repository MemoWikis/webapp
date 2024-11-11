public class PageCreator : IRegisterAsInstancePerLifetime
{
    private readonly Logg _logg;
    private readonly PageRepository _pageRepository;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly PageRelationRepo _pageRelationRepo;

    public PageCreator(
        Logg logg,
        PageRepository pageRepository,
        UserReadingRepo userReadingRepo,
        PageRelationRepo pageRelationRepo)
    {
        _logg = logg;
        _pageRepository = pageRepository;
        _userReadingRepo = userReadingRepo;
        _pageRelationRepo = pageRelationRepo;
    }

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

        var topic = new Page(name, sessionUser.UserId);

        topic.Creator = _userReadingRepo.GetById(sessionUser.UserId);
        topic.Type = PageType.Standard;
        topic.Visibility = PageVisibility.Owner;
        _pageRepository.Create(topic);

        var modifyRelationsForCategory = new ModifyRelationsForCategory(_pageRepository, _pageRelationRepo);
        modifyRelationsForCategory.AddChild(parentPageId, topic.Id, sessionUser.UserId);

        return new CreateResult
        {
            Success = true,
            Data = new TinyPageItem
            {
                Name = topic.Name,
                Id = topic.Id
            }
        };
    }
}