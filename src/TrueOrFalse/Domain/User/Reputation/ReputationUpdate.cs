using System.Linq;

public class ReputationUpdate : IRegisterAsInstancePerLifetime
{
    private readonly JobQueueRepo _jobQueueRepo;
 

    public ReputationUpdate(JobQueueRepo jobQueueRepo)
    {
        _jobQueueRepo = jobQueueRepo;
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

 
}