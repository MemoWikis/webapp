using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using RollbarSharp;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class KnowledgeReportCheck : IJob
    {
        private readonly JobQueueRepo _jobQueueRepo;
        private readonly UserRepo _userRepo;
        private readonly MessageEmailRepo _messageEmailRepo;
        private readonly GetAnswerStatsInPeriod _getAnswerStatsInPeriod;
        private readonly GetStreaksDays _getStreaksDays;
        private readonly GetUnreadMessageCount _getUnreadMessageCount;
        private readonly KnowledgeSummaryLoader _knowledgeSummaryLoader;
        private readonly QuestionReadingRepo _questionReadingRepo;

        public KnowledgeReportCheck(JobQueueRepo jobQueueRepo,
            UserRepo userRepo,
            MessageEmailRepo messageEmailRepo,
            GetAnswerStatsInPeriod getAnswerStatsInPeriod,
            GetStreaksDays getStreaksDays,
            GetUnreadMessageCount getUnreadMessageCount,
            KnowledgeSummaryLoader knowledgeSummaryLoader,
            QuestionReadingRepo questionReadingRepo)
        {
            _jobQueueRepo = jobQueueRepo;
            _userRepo = userRepo;
            _messageEmailRepo = messageEmailRepo;
            _getAnswerStatsInPeriod = getAnswerStatsInPeriod;
            _getStreaksDays = getStreaksDays;
            _getUnreadMessageCount = getUnreadMessageCount;
            _knowledgeSummaryLoader = knowledgeSummaryLoader;
            _questionReadingRepo = questionReadingRepo;
        }
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                var users = _userRepo.GetAll().Where(u => u.WishCountQuestions > 1);
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
                            _userRepo, 
                            _getUnreadMessageCount, 
                            _knowledgeSummaryLoader,
                            _questionReadingRepo);
                    }
                }

            }, "KnowledgeReportCheck");
        }

    }
}