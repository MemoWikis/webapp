using System.Linq;

public class UpdateReputationsAndRank : IRegisterAsInstancePerLifetime
{
    private readonly UserRepository _userRepository;
    private readonly ReputationCalc _reputationCalc;

    public UpdateReputationsAndRank(
        UserRepository userRepository,
        ReputationCalc reputationCalc)
    {
        _userRepository = userRepository;
        _reputationCalc = reputationCalc;
    }

    public void Run()
    {
        var allUsers = _userRepository.GetAll();
        var results = allUsers
            .Select(user => _reputationCalc.Run(user))
            .OrderByDescending(r => r.TotalRepuation);

        var i = 0;
        foreach (var result in results)
        {
            i++;
            result.User.ReputationPos = i;
            result.User.Reputation = result.TotalRepuation;
            result.User.WishCountQuestions = Sl.Resolve<GetWishQuestionCount>().Run(result.User.Id);
            result.User.WishCountSets = Sl.Resolve<GetWishSetCount>().Run(result.User.Id);

            _userRepository.Update(result.User);
        }
    }
}