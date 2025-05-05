public class UpdateWishcount : IRegisterAsInstancePerLifetime
{
    private readonly UserReadingRepo _userReadingRepo;
    private readonly GetWishQuestionCount _getWishQuestionCount;
    private readonly UserWritingRepo _userWritingRepo;

    public UpdateWishcount(
        UserReadingRepo userReadingRepo,
        GetWishQuestionCount getWishQuestionCount,
        UserWritingRepo userWritingRepo)
    {
        _userReadingRepo = userReadingRepo;
        _getWishQuestionCount = getWishQuestionCount;
        _userWritingRepo = userWritingRepo;
    }

    public void Run()
    {
        Run(_userReadingRepo.GetAll());
    }

    public void Run(IEnumerable<User> users)
    {
        foreach (var user in users)
        {
            Run(user);
        }
    }

    public void Run(User user)
    {
        user.WishCountQuestions = _getWishQuestionCount.Run(user.Id);
        _userWritingRepo.Update(user);
    }
}