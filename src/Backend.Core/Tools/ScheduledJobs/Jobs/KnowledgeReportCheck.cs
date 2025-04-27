using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Quartz;

public class KnowledgeReportCheck : IJob
{
    private readonly JobQueueRepo _jobQueueRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly MessageEmailRepo _messageEmailRepo;
    private readonly GetAnswerStatsInPeriod _getAnswerStatsInPeriod;
    private readonly GetStreaksDays _getStreaksDays;
    private readonly GetUnreadMessageCount _getUnreadMessageCount;
    private readonly KnowledgeSummaryLoader _knowledgeSummaryLoader;
    private readonly QuestionReadingRepo _questionReadingRepo;
    private readonly HttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public KnowledgeReportCheck(JobQueueRepo jobQueueRepo,
        UserReadingRepo userReadingRepo,
        MessageEmailRepo messageEmailRepo,
        GetAnswerStatsInPeriod getAnswerStatsInPeriod,
        GetStreaksDays getStreaksDays,
        GetUnreadMessageCount getUnreadMessageCount,
        KnowledgeSummaryLoader knowledgeSummaryLoader,
        QuestionReadingRepo questionReadingRepo,
        HttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _jobQueueRepo = jobQueueRepo;
        _userReadingRepo = userReadingRepo;
        _messageEmailRepo = messageEmailRepo;
        _getAnswerStatsInPeriod = getAnswerStatsInPeriod;
        _getStreaksDays = getStreaksDays;
        _getUnreadMessageCount = getUnreadMessageCount;
        _knowledgeSummaryLoader = knowledgeSummaryLoader;
        _questionReadingRepo = questionReadingRepo;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }

    public Task Execute(IJobExecutionContext context)
    {
        JobExecute.Run(scope =>
        {
            var users = _userReadingRepo.GetAll().Where(u => u.WishCountQuestions > 1);
            foreach (var user in users)
            {
                if (KnowledgeReportMsg.ShouldSendToUser(user, _messageEmailRepo))
                {
                    Logg.r.Information("Sending Knowledge-Report to user " + user.Name + " (" + user.Id + ")...");
                    KnowledgeReportMsg.SendHtmlMail(user,
                        _jobQueueRepo, 
                        _messageEmailRepo, 
                        _getAnswerStatsInPeriod, 
                        _getStreaksDays, 
                        _userReadingRepo, 
                        _getUnreadMessageCount, 
                        _knowledgeSummaryLoader,
                        _questionReadingRepo,
                        _httpContextAccessor,
                        _webHostEnvironment);
                }
            }
        }, "KnowledgeReportCheck");

        return Task.CompletedTask;
    }
}