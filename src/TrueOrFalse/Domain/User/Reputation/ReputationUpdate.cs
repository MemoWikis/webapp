using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class ReputationUpdate : IRegisterAsInstancePerLifetime
{
    private readonly JobQueueRepo _jobQueueRepo;
 

    public ReputationUpdate(JobQueueRepo jobQueueRepo)
    {
        _jobQueueRepo = jobQueueRepo;
    }

    public void ForQuestion(int questionId,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment) =>
      ScheduleUpdate(EntityCache.GetQuestionById(questionId, httpContextAccessor, webHostEnvironment)
          .Creator(httpContextAccessor, webHostEnvironment).Id);


    public void ForUser(User user) =>
      ScheduleUpdate(user.Id);

    public void ForUser(UserCacheItem user) =>
      ScheduleUpdate(user.Id);

    public void ForUser(UserTinyModel user) =>
      ScheduleUpdate(user.Id);

    private void ScheduleUpdate(int userId) =>
      _jobQueueRepo.Add(JobQueueType.UpdateReputationForUser, userId.ToString());

 
}