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
        TinyTopicItem Data);

    public readonly record struct TinyTopicItem(bool CantSavePrivateTopic, string Name, int Id);

    public CreateResult Create(string name, int parentTopicId, SessionUser sessionUser)
    {
        if (!new LimitCheck(_logg, sessionUser).CanSavePrivateTopic(true))
        {
            return new CreateResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Subscription.CantSavePrivateTopic,
                Data = new TinyTopicItem
                {
                    CantSavePrivateTopic = true
                }
            };
        }

        var topic = new Page(name, sessionUser.UserId);

        topic.Creator = _userReadingRepo.GetById(sessionUser.UserId);
        topic.Type = PageType.Standard;
        topic.Visibility = PageVisibility.Owner;
        _pageRepository.Create(topic);

        var modifyRelationsForCategory = new ModifyRelationsForCategory(_pageRepository, _pageRelationRepo);
        modifyRelationsForCategory.AddChild(parentTopicId, topic.Id, sessionUser.UserId);

        return new CreateResult
        {
            Success = true,
            Data = new TinyTopicItem
            {
                Name = topic.Name,
                Id = topic.Id
            }
        };
    }
}