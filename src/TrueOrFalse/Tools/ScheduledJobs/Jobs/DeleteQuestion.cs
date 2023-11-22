using Quartz;
using ISession = NHibernate.ISession;

namespace TrueOrFalse.Utilities.ScheduledJobs;

public class DeleteQuestion : IJob
{
    private readonly ReferenceRepo _referenceRepo;
    private readonly AnswerRepo _answerRepo;
    private readonly QuestionViewRepository _questionViewRepository;
    private readonly UserActivityRepo _userActivityRepo;
    private readonly CommentRepository _commentRepository;
    private readonly ISession _nhibernateSession;
    private readonly QuestionWritingRepo _questionWritingRepo;
    private readonly QuestionValuationWritingRepo _questionValuationWritingRepo;


    public DeleteQuestion(ReferenceRepo referenceRepo,
        AnswerRepo answerRepo,
        QuestionViewRepository questionViewRepository,
        UserActivityRepo userActivityRepo,
        CommentRepository commentRepository,
        ISession nhibernateSession,
        QuestionWritingRepo questionWritingRepo,
        QuestionValuationWritingRepo questionValuationWritingRepo)
    {
        _referenceRepo = referenceRepo;
        _answerRepo = answerRepo;
        _questionViewRepository = questionViewRepository;
        _userActivityRepo = userActivityRepo;
        _commentRepository = commentRepository;
        _nhibernateSession = nhibernateSession;
        _questionWritingRepo = questionWritingRepo;
        _questionValuationWritingRepo = questionValuationWritingRepo;
    }
    public Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;
        var questionId = dataMap.GetInt("questionId");
        var userId = dataMap.GetInt("userId");
        Logg.r.Information("Job started - DeleteQuestion {id}", questionId);

        //delete connected db-entries
        _referenceRepo.DeleteForQuestion(questionId);
        _answerRepo.DeleteFor(questionId);
        _questionViewRepository.DeleteForQuestion(questionId);
        _userActivityRepo.DeleteForQuestion(questionId);
        _questionViewRepository.DeleteForQuestion(questionId);
        _questionValuationWritingRepo.DeleteForQuestion(questionId);
        _commentRepository.DeleteForQuestion(questionId);
        _nhibernateSession
            .CreateSQLQuery("DELETE FROM categories_to_questions where Question_id = " + questionId)
            .ExecuteUpdate();

        var categoriesToUpdateIds = _questionWritingRepo.Delete(questionId, userId);
        JobScheduler.StartImmediately_UpdateAggregatedCategoriesForQuestion(categoriesToUpdateIds);
        Logg.r.Information("Question {id} deleted", questionId);

        return Task.CompletedTask;
    }
}