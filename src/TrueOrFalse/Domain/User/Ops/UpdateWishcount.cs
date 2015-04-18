using System.Collections.Generic;

public class UpdateWishcount : IRegisterAsInstancePerLifetime
{
    private readonly UserRepository _userRepository;
    private readonly GetWishQuestionCount _getWishQuestionCount;
    private readonly GetWishSetCount _getWishSetCount;

    public UpdateWishcount(
        UserRepository userRepository,
        GetWishQuestionCount getWishQuestionCount,
        GetWishSetCount getWishSetCount)
    {
        _userRepository = userRepository;
        _getWishQuestionCount = getWishQuestionCount;
        _getWishSetCount = getWishSetCount;
    }

    public void Run()
    {
        Run(_userRepository.GetAll());
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
        _userRepository.Update(user);
    }
}