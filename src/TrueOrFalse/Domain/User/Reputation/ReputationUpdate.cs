using System.Collections.Generic;

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

    public void ForQuestion(int questionId)
    {
        Run(Sl.Resolve<QuestionRepo>().GetById(questionId).Creator);
    }

    public void ForSet(int setId)
    {
        Run(Sl.Resolve<SetRepo>().GetById(setId).Creator);
    }

    public static void ScheduleUpdate(IList<int> userIds)
    {
        Sl.R<JobQueueRepo>().Add(JobQueueType.UpdateReputationForUsers, string.Join(",", userIds));
    }

    public static void ScheduleUpdate(int userId)
    {
        Sl.R<JobQueueRepo>().Add(JobQueueType.UpdateReputationForUsers, userId.ToString());
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
}