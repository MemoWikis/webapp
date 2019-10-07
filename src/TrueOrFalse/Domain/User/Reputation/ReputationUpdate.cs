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
        ScheduleUpdate(Sl.QuestionRepo.GetById(questionId).Creator.Id);

    public static void ForSet(int setId) => 
        ScheduleUpdate(Sl.SetRepo.GetById(setId).Creator.Id);

    public static void ForCategory(int categoryId) => 
        ScheduleUpdate(Sl.CategoryRepo.GetById(categoryId).Creator.Id);

    public static void ForUser(User user) => 
        ScheduleUpdate(user.Id);

    private static void ScheduleUpdate(int userId) => 
        Sl.R<JobQueueRepo>().Add(JobQueueType.UpdateReputationForUser, userId.ToString());

    public void Run(User userToUpdate)
    {
        var oldReputation = userToUpdate.Reputation;
        var newReputation  = userToUpdate.Reputation = _reputationCalc.Run(userToUpdate).TotalReputation;

        var users = _userRepo.GetWhereReputationIsBetween(newReputation, oldReputation);
        foreach (User user in users)
        {
            userToUpdate.ReputationPos = user.ReputationPos;
            if (newReputation < oldReputation)
                user.ReputationPos--;
            else
                user.ReputationPos++;

            _userRepo.Update(user, runSolrUpdateAsync: true);
        }

        _userRepo.Update(userToUpdate, runSolrUpdateAsync:true);
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