using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Quartz;
using ISession = NHibernate.ISession;

namespace TrueOrFalse.Utilities.ScheduledJobs;

public class DeleteQuestion : IJob
{
    private readonly CategoryValuationReadingRepo _categoryValuationReadingRepo;
    private readonly ReferenceRepo _referenceRepo;
    private readonly AnswerRepo _answerRepo;
    private readonly QuestionViewRepository _questionViewRepository;
    private readonly UserActivityRepo _userActivityRepo;
    private readonly QuestionValuationRepo _questionValuationRepo;
    private readonly CommentRepository _commentRepository;
    private readonly ISession _nhibernateSession;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly QuestionWritingRepo _questionWritingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public DeleteQuestion(CategoryValuationReadingRepo categoryValuationReadingRepo,
        ReferenceRepo referenceRepo,
        AnswerRepo answerRepo,
        QuestionViewRepository questionViewRepository,
        UserActivityRepo userActivityRepo,
        QuestionValuationRepo questionValuationRepo,
        CommentRepository commentRepository,
        ISession nhibernateSession,
        UserReadingRepo userReadingRepo, 
        QuestionWritingRepo questionWritingRepo,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _categoryValuationReadingRepo = categoryValuationReadingRepo;
        _referenceRepo = referenceRepo;
        _answerRepo = answerRepo;
        _questionViewRepository = questionViewRepository;
        _userActivityRepo = userActivityRepo;
        _questionValuationRepo = questionValuationRepo;
        _commentRepository = commentRepository;
        _nhibernateSession = nhibernateSession;
        _userReadingRepo = userReadingRepo;
        _questionWritingRepo = questionWritingRepo;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }
    public Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;
        var questionId = dataMap.GetInt("questionId");
        new Logg(_httpContextAccessor, _webHostEnvironment).r().Information("Job started - DeleteQuestion {id}", questionId);

        SessionUserCache.RemoveQuestionForAllUsers(questionId, _categoryValuationReadingRepo, _userReadingRepo, _questionValuationRepo);

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
        new Logg(_httpContextAccessor, _webHostEnvironment).r().Information("Question {id} deleted", questionId);

        return Task.CompletedTask;
    }
}