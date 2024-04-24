public class CategoryCreator(
    Logg _logg,
    CategoryRepository _categoryRepository,
    UserReadingRepo _userReadingRepo,
    CategoryRelationRepo _categoryRelationRepo) : IRegisterAsInstancePerLifetime
{
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

        var modifyRelationsForCategory =
            new ModifyRelationsForCategory(_categoryRepository, _categoryRelationRepo);
        modifyRelationsForCategory.AddChild(parentTopicId, topic.Id);

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