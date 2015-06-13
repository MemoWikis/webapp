using System.Collections.Generic;

public class UpdateWishcount : IRegisterAsInstancePerLifetime
{
    private readonly UserRepo _userRepo;
    private readonly GetWishQuestionCount _getWishQuestionCount;
    private readonly GetWishSetCount _getWishSetCount;

    public UpdateWishcount(
        UserRepo userRepo,
        GetWishQuestionCount getWishQuestionCount,
        GetWishSetCount getWishSetCount)
    {
        _userRepo = userRepo;
        _getWishQuestionCount = getWishQuestionCount;
        _getWishSetCount = getWishSetCount;
    }

    public void Run()
    {
        Run(_userRepo.GetAll());
    }

    public void Run(IEnumerable<User> users)
    {
        foreach (var user in users)
            Run(user);
    }

    public void Run(User user)
    {
        user.WishCountQuestions = _getWishQuestionCount.Run(user.Id);
        user.WishCountSets = _getWishSetCount.Run(user.Id);
        _userRepo.Update(user);
    }
}