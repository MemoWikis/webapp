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
    private readonly QuestionValuationRepo _questionValuationRepo;
    private readonly CommentRepository _commentRepository;
    private readonly ISession _nhibernateSession;
    private readonly QuestionWritingRepo _questionWritingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly SessionUserCache _sessionUserCache;
    private readonly Logg _logg;

    public DeleteQuestion(ReferenceRepo referenceRepo,
        AnswerRepo answerRepo,
        QuestionViewRepository questionViewRepository,
        UserActivityRepo userActivityRepo,
        QuestionValuationRepo questionValuationRepo,
        CommentRepository commentRepository,
        ISession nhibernateSession,
        QuestionWritingRepo questionWritingRepo,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        SessionUserCache sessionUserCache,
        Logg logg)
    {
        _referenceRepo = referenceRepo;
        _answerRepo = answerRepo;
        _questionViewRepository = questionViewRepository;
        _userActivityRepo = userActivityRepo;
        _questionValuationRepo = questionValuationRepo;
        _commentRepository = commentRepository;
        _nhibernateSession = nhibernateSession;
        _questionWritingRepo = questionWritingRepo;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _sessionUserCache = sessionUserCache;
        _logg = logg;
    }
    public Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;
        var questionId = dataMap.GetInt("questionId");
        new Logg(_httpContextAccessor, _webHostEnvironment).r().Information("Job started - DeleteQuestion {id}", questionId);

        _sessionUserCache.RemoveQuestionForAllUsers(questionId);

        //delete connected db-entries
        _referenceRepo.DeleteForQuestion(questionId);
        _answerRepo.DeleteFor(questionId);
        _questionViewRepository.DeleteForQuestion(questionId);
        _userActivityRepo.DeleteForQuestion(questionId);
        _questionViewRepository.DeleteForQuestion(questionId);
        _questionValuationRepo.DeleteForQuestion(questionId);
        _commentRepository.DeleteForQuestion(questionId);
        _nhibernateSession
            .CreateSQLQuery("DELETE FROM categories_to_questions where Question_id = " + questionId)
            .ExecuteUpdate();

        var categoriesToUpdateIds = _questionWritingRepo.Delete(questionId);
        JobScheduler.StartImmediately_UpdateAggregatedCategoriesForQuestion(categoriesToUpdateIds);
        _logg.r().Information("Question {id} deleted", questionId);

        return Task.CompletedTask;
    }
}