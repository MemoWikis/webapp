using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly QuestionValuationWritingRepo _questionValuationWritingRepo;
    private readonly SessionUserCache _sessionUserCache;
    private readonly Logg _logg;


    public DeleteQuestion(ReferenceRepo referenceRepo,
        AnswerRepo answerRepo,
        QuestionViewRepository questionViewRepository,
        UserActivityRepo userActivityRepo,
        CommentRepository commentRepository,
        ISession nhibernateSession,
        QuestionWritingRepo questionWritingRepo,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        Logg logg,
        QuestionValuationWritingRepo questionValuationWritingRepo)
    {
        _referenceRepo = referenceRepo;
        _answerRepo = answerRepo;
        _questionViewRepository = questionViewRepository;
        _userActivityRepo = userActivityRepo;
        _commentRepository = commentRepository;
        _nhibernateSession = nhibernateSession;
        _questionWritingRepo = questionWritingRepo;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _questionValuationWritingRepo = questionValuationWritingRepo;
        _sessionUserCache = _questionValuationWritingRepo.SessionUserCache;
        _logg = logg;
    }
    public Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;
        var questionId = dataMap.GetInt("questionId");
        Logg.r.Information("Job started - DeleteQuestion {id}", questionId);

        _sessionUserCache.RemoveQuestionForAllUsers(questionId);

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

        var categoriesToUpdateIds = _questionWritingRepo.Delete(questionId);
        JobScheduler.StartImmediately_UpdateAggregatedCategoriesForQuestion(categoriesToUpdateIds);
        Logg.r.Information("Question {id} deleted", questionId);

        return Task.CompletedTask;
    }
}