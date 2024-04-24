public class CategoryCreator : IRegisterAsInstancePerLifetime
{
    private readonly Logg _logg;
    private readonly CategoryRepository _categoryRepository;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly CategoryRelationRepo _categoryRelationRepo;

    public CategoryCreator(
        Logg logg,
        CategoryRepository categoryRepository,
        UserReadingRepo userReadingRepo,
        CategoryRelationRepo categoryRelationRepo)
    {
        _logg = logg;
        _categoryRepository = categoryRepository;
        _userReadingRepo = userReadingRepo;
        _categoryRelationRepo = categoryRelationRepo;
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

        var topic = new Category(name, sessionUser.UserId);

        topic.Creator = _userReadingRepo.GetById(sessionUser.UserId);
        topic.Type = CategoryType.Standard;
        topic.Visibility = CategoryVisibility.Owner;
        _categoryRepository.Create(topic);

        var modifyRelationsForCategory =
            new ModifyRelationsForCategory(_categoryRepository, _categoryRelationRepo);
        modifyRelationsForCategory.AddChildAsync(parentTopicId, topic.Id);

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