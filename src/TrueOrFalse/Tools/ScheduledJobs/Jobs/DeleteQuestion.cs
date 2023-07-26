using System.Linq;
using NHibernate;
using Quartz;

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
    private readonly UserRepo _userRepo;
    private readonly QuestionWritingRepo _questionWritingRepo;

    public DeleteQuestion(CategoryValuationReadingRepo categoryValuationReadingRepo,
        ReferenceRepo referenceRepo,
        AnswerRepo answerRepo,
        QuestionViewRepository questionViewRepository,
        UserActivityRepo userActivityRepo,
        QuestionValuationRepo questionValuationRepo,
        CommentRepository commentRepository,
        ISession nhibernateSession,
        UserRepo userRepo, QuestionWritingRepo questionWritingRepo)
    {
        _categoryValuationReadingRepo = categoryValuationReadingRepo;
        _referenceRepo = referenceRepo;
        _answerRepo = answerRepo;
        _questionViewRepository = questionViewRepository;
        _userActivityRepo = userActivityRepo;
        _questionValuationRepo = questionValuationRepo;
        _commentRepository = commentRepository;
        _nhibernateSession = nhibernateSession;
        _userRepo = userRepo;
        _questionWritingRepo = questionWritingRepo;
    }
    public void Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;
        var questionId = dataMap.GetInt("questionId");
        Logg.r().Information("Job started - DeleteQuestion {id}", questionId);

        SessionUserCache.RemoveQuestionForAllUsers(questionId, _categoryValuationReadingRepo, _userRepo, _questionValuationRepo);

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
        Logg.r().Information("Question {id} deleted", questionId);
    }
}