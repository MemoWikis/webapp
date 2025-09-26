using ISession = NHibernate.ISession;

/// <summary>
/// Updates the probabilities 
///     - for all question valuations 
///     - for all useres
/// </summary>
public class ProbabilityUpdate_ValuationAll(
    ISession _nhibernateSession,
    QuestionReadingRepo _questionReadingRepo,
    ProbabilityCalc_Simple1 _probabilityCalcSimple1,
    AnswerRepo _answerRepo,
    UserReadingRepo _userReadingRepo,
    QuestionValuationReadingRepo _questionValuationReadingRepo,
    ExtendedUserCache _extendedUserCache)
    : IRegisterAsInstancePerLifetime
{
    public void Run(string? jobTrackingId = null)
    {
        var questionValuationRecords =
            _nhibernateSession.QueryOver<QuestionValuation>()
                .Where(qv => qv.User != null && qv.Question != null)
                .Select(
                    qv => qv.Question.Id,
                    qv => qv.User.Id)
                .List<object[]>();

        foreach (var item in questionValuationRecords)
        {
            var questionId = (int)item[0];
            var userId = (int)item[1];

            if (jobTrackingId != null)
            {
                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running,
                    $"Updating valuation for Question ID {questionId}, User ID {userId}...",
                    "ProbabilityUpdate_ValuationAll");
            }

            new ProbabilityUpdate_Valuation(_nhibernateSession,
                    _questionValuationReadingRepo,
                    _probabilityCalcSimple1,
                    _answerRepo,
                    _extendedUserCache)
                .Run(questionId,
                    userId,
                    _questionReadingRepo,
                    _userReadingRepo);
        }
    }
}