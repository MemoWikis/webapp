using System.Linq;

public class UpdateReputationsAndRank : IRegisterAsInstancePerLifetime
{
    private readonly UserRepo _userRepo;
    private readonly ReputationCalc _reputationCalc;

    public UpdateReputationsAndRank(
        UserRepo userRepo,
        ReputationCalc reputationCalc)
    {
        _userRepo = userRepo;
        _reputationCalc = reputationCalc;
    }

    public void Run()
    {
        var allUsers = _userRepo.GetAll();
        var results = allUsers
            .Select(user => _reputationCalc.Run(user))
            .OrderByDescending(r => r.TotalReputation);

        var i = 0;
        foreach (var result in results)
        {
            i++;
            result.User.ReputationPos = i;
            result.User.Reputation = result.TotalReputation;
            result.User.WishCountQuestions = Sl.Resolve<GetWishQuestionCount>().Run(result.User.Id);
            result.User.WishCountSets = Sl.Resolve<GetWishSetCount>().Run(result.User.Id);

            _userRepo.Update(result.User);
        }
    }
}