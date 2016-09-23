using System.Collections.Generic;
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

    public static void ForQuestion(int questionId)
    {
        ScheduleUpdate(Sl.Resolve<QuestionRepo>().GetById(questionId).Creator.Id);
    }

    public static void ForSet(int setId)
    {
        ScheduleUpdate(Sl.Resolve<SetRepo>().GetById(setId).Creator.Id);
    }

    public static void ForUser(User user)
    {
        ScheduleUpdate(user.Id);
    }

    private static void ScheduleUpdate(int userId)
    {
        Sl.R<JobQueueRepo>().Add(JobQueueType.UpdateReputationForUser, userId.ToString());
    }

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

    //public void Run(IList<int> userIds)
    //{
    //    var uniqueUserIds = userIds.Distinct().ToList();
    //    foreach (var userId in uniqueUserIds)
    //    {
    //        Run(Sl.R<UserRepo>().GetById(userId));
    //    }

    //}
}