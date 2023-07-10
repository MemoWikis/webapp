using System.Linq;

public class ReputationUpdate : IRegisterAsInstancePerLifetime
{
    private readonly ReputationCalc _reputationCalc;
    private readonly UserRepo _userRepo;
    private readonly JobQueueRepo _jobQueueRepo;
    private readonly GetWishQuestionCount _getWishQuestionCount;

    public ReputationUpdate(
      ReputationCalc reputationCalc,
      UserRepo userRepo,
      JobQueueRepo jobQueueRepo,
      GetWishQuestionCount getWishQuestionCount)
    {
        _reputationCalc = reputationCalc;
        _userRepo = userRepo;
        _jobQueueRepo = jobQueueRepo;
        _getWishQuestionCount = getWishQuestionCount;
    }

    public void ForQuestion(int questionId) =>
      ScheduleUpdate(EntityCache.GetQuestionById(questionId).Creator.Id);


    public void ForUser(User user) =>
      ScheduleUpdate(user.Id);

    public void ForUser(UserCacheItem user) =>
      ScheduleUpdate(user.Id);

    public void ForUser(UserTinyModel user) =>
      ScheduleUpdate(user.Id);

    private void ScheduleUpdate(int userId) =>
      _jobQueueRepo.Add(JobQueueType.UpdateReputationForUser, userId.ToString());

    public void Run(User userToUpdate)
    {
        var userToUpdateCacheItem = EntityCache.GetUserById(userToUpdate.Id);

        var oldReputation = userToUpdate.Reputation;
        var newReputation = userToUpdate.Reputation = _reputationCalc.Run(userToUpdateCacheItem).TotalReputation;

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
        var allUsers = UserCacheItem.ToCacheUsers(_userRepo.GetAll());

        var results = allUsers
          .Select(user => _reputationCalc.Run(user))
          .OrderByDescending(r => r.TotalReputation);

        var i = 0;
        foreach (var result in results)
        {
            i++;
            result.User.User.ReputationPos = i;
            result.User.User.Reputation = result.TotalReputation;
            result.User.User.WishCountQuestions = _getWishQuestionCount.Run(result.User.Id);
            //result.User.User.WishCountSets = Sl.Resolve<GetWishSetCount>().Run(result.User.Id);

            _userRepo.Update(result.User.User);
        }
    }
}