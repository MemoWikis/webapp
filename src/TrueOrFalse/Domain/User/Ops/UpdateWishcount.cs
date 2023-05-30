using System.Collections.Generic;

public class UpdateWishcount : IRegisterAsInstancePerLifetime
{
    private readonly UserRepo _userRepo;
    private readonly GetWishQuestionCount _getWishQuestionCount;

    public void Run()
    {
        Run(_userRepo.GetAll());
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
        _userRepo.Update(user);
    }
}