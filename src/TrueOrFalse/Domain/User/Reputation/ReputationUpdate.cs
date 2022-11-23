using System.Linq;

public class ReputationUpdate : IRegisterAsInstancePerLifetime
{
    private readonly ReputationCalc _reputationCalc;
    private readonly UserRepo _userRepo;

    public ReputationUpdate(
        ReputationCalc reputationCalc,
        UserRepo userRepo)
    {
        _reputationCalc = reputationCalc;
        _userRepo = userRepo;
    }

    public static void ForQuestion(int questionId) => 
        ScheduleUpdate(EntityCache.GetQuestionById(questionId).Creator.UserId);


    public static void ForUser(User user) =>
        ScheduleUpdate(user.Id);

    public static void ForUser(UserTinyModel user) =>
        ScheduleUpdate(user.Id);

    private static void ScheduleUpdate(int userId) =>
        Sl.JobQueueRepo.Add(JobQueueType.UpdateReputationForUser, userId.ToString());

    public void Run(int userToUpdateId)
    {
        var userToUpdate = SessionUserCache.GetItem(userToUpdateId);

        var oldReputation = userToUpdate.Reputation;
        var newReputation = userToUpdate.Reputation = _reputationCalc.Run(userToUpdate).TotalReputation;

        var users = _userRepo.GetWhereReputationIsBetween(newReputation, oldReputation);
        foreach (User user in users)
        {
            userToUpdate.ReputationPos = user.ReputationPos;
            if (newReputation < oldReputation)
                user.ReputationPos--;
            else
                user.ReputationPos++;

            _userRepo.Update(user);
        }

        _userRepo.Update(userToUpdate);
    }

    public void RunForAll()
    {
        var allUsers = _userRepo.GetAll();
        var results = allUsers
            .Select(user => _reputationCalc.Run(user))
            .OrderByDescending(r => r.TotalReputation);

        var i = 0;
        foreach (var result in results)
        {
            i++;
            result.User.User.ReputationPos = i;
            result.User.User.Reputation = result.TotalReputation;
            result.User.User.WishCountQuestions = Sl.Resolve<GetWishQuestionCount>().Run(result.User.Id);
            result.User.User.WishCountSets = Sl.Resolve<GetWishSetCount>().Run(result.User.Id);

            _userRepo.Update(result.User.User);
        }
    }
}