using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
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

        public KnowledgeReportCheck(JobQueueRepo jobQueueRepo,
            UserReadingRepo userReadingRepo,
            MessageEmailRepo messageEmailRepo,
            GetAnswerStatsInPeriod getAnswerStatsInPeriod,
            GetStreaksDays getStreaksDays,
            GetUnreadMessageCount getUnreadMessageCount,
            KnowledgeSummaryLoader knowledgeSummaryLoader,
            QuestionReadingRepo questionReadingRepo)
        {
            _jobQueueRepo = jobQueueRepo;
            _userReadingRepo = userReadingRepo;
            _messageEmailRepo = messageEmailRepo;
            _getAnswerStatsInPeriod = getAnswerStatsInPeriod;
            _getStreaksDays = getStreaksDays;
            _getUnreadMessageCount = getUnreadMessageCount;
            _knowledgeSummaryLoader = knowledgeSummaryLoader;
            _questionReadingRepo = questionReadingRepo;
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
                        Logg.r().Information("Sending Knowledge-Report to user " + user.Name + " (" + user.Id + ")...");
                        KnowledgeReportMsg.SendHtmlMail(user,
                            _jobQueueRepo, 
                            _messageEmailRepo, 
                            _getAnswerStatsInPeriod, 
                            _getStreaksDays, 
                            _userReadingRepo, 
                            _getUnreadMessageCount, 
                            _knowledgeSummaryLoader,
                            _questionReadingRepo);
                    }
                }

            }, "KnowledgeReportCheck");

            return Task.CompletedTask;
        }

    }
}