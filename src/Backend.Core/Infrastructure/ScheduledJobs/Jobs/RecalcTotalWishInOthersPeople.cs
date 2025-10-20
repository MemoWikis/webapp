using NHibernate;
using Quartz;
using System.Net.Mail;

public class RecalcTotalWishInOthersPeople : IJob
{
    private readonly JobQueueRepo _jobQueueRepo;
    private readonly ISession _nhibernateSession;
    private readonly UserReadingRepo _userReadingRepo;

    public RecalcTotalWishInOthersPeople(JobQueueRepo jobQueueRepo,
        ISession nhibernateSession,
        UserReadingRepo userReadingRepo)
    {
        _jobQueueRepo = jobQueueRepo;
        _nhibernateSession = nhibernateSession;
        _userReadingRepo = userReadingRepo;
    }
    public Task Execute(IJobExecutionContext context)
    {
        JobExecute.Run(scope =>
        {
            var start = DateTime.Now;
            var report = GetReport();

            _nhibernateSession.CreateSQLQuery(
                @"UPDATE user SET TotalInOthersWishKnowledge = (
                        SELECT count(*) FROM questionvaluation qv
                        JOIN question q
                        ON qv.Questionid = q.Id
                        WHERE qv.RelevancePersonal > 0
                        AND q.Creator_id = user.Id
                        AND qv.UserId <> user.Id);"
            ).ExecuteUpdate();

            var end = DateTime.Now;
            report += (end - start) + " Sekunden.";

            SendMail("daniel.majunke@googlemail.com", "Daniel", report);
            SendMail("robert@robert - m.de", "Robert", report);

        }, "RecalcTotalWishInOthersPeople");

        return Task.CompletedTask;
    }

    private string GetReport()
    {
        var userIds = _userReadingRepo.GetAllIds();
        var counter = 0;

        foreach (var userId in userIds)
        {
            var userTotalWishKnowledgeInOtherPoeple = _nhibernateSession
                .CreateSQLQuery(@"Select TotalInOthersWishKnowledge From User where Id = :userId ")
                .SetParameter("userId", userId)
                .UniqueResult<int>();

            var joinTotalWishKnowledgeInOtherPoeple = _nhibernateSession.CreateSQLQuery(
                    @"SELECT count(qv.Id) FROM questionvaluation qv JOIN 
                    question q ON qv.Questionid = q.Id
                    WHERE qv.RelevancePersonal > 0
                    AND q.Creator_id = :userId
                    AND qv.UserId <> :userId; ")
                .SetParameter("userId", userId)
                .UniqueResult<long>();

            if (userTotalWishKnowledgeInOtherPoeple != joinTotalWishKnowledgeInOtherPoeple)
                counter++;
        }

        return counter +
               " Zahlen unterscheiden sich bei der User Tabelle mit der Spalte TotalInOthersWishKnowledge, von dem Join über die entsprechenden Tabellen und dauerte ";
    }

    private void SendMail(string to, string name, string report)
    {
        SendEmail.Run(new MailMessage("daniel.majunke@googlemail.com", to,
            "Report TotalWishKnowledge in other people",
            $"Hallo {name}, hier die gewünschten Zahlen: {report}"), _jobQueueRepo, _userReadingRepo);
    }
}