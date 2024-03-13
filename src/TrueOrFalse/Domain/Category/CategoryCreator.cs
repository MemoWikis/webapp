using TrueOrFalse.Utilities.ScheduledJobs;

public class CategoryCreator : IRegisterAsInstancePerLifetime
{
    private readonly Logg _logg;
    private readonly CategoryRepository _categoryRepository;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly CategoryRelationRepo _categoryRelationRepo;


    public CategoryCreator(Logg logg, CategoryRepository categoryRepository, UserReadingRepo userReadingRepo, CategoryRelationRepo categoryRelationRepo)
    {
        _logg = logg;
        _categoryRepository = categoryRepository;
        _userReadingRepo = userReadingRepo;
        _categoryRelationRepo = categoryRelationRepo;
    }

    public RequestResult Create(string name, int parentTopicId, SessionUser sessionUser)
    {
        if (!new LimitCheck(_logg, sessionUser).CanSavePrivateTopic(true))
        {
            return new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Subscription.CantSavePrivateTopic,
                data = new
                {
                    cantSavePrivateTopic = true
                }
            };
        }

        var topic = new Category(name, sessionUser.UserId);

        topic.Creator = _userReadingRepo.GetById(sessionUser.UserId);
        topic.Type = CategoryType.Standard;
        topic.Visibility = CategoryVisibility.Owner;
        _categoryRepository.Create(topic);

        var newRelation = ModifyRelationsEntityCache.AddChild(parentTopicId, topic.Id);
        JobScheduler.StartImmediately_ModifyTopicRelations(new List<CategoryCacheRelation> { newRelation }, sessionUser.UserId);

        return new RequestResult
        {
            success = true,
            data = new
            {
                name = topic.Name,
                id = topic.Id
            }
        };
    }
}