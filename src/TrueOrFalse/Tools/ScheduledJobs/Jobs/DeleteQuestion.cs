using System.Linq;
using NHibernate;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs;

public class DeleteQuestion : IJob
{
    private readonly CategoryValuationRepo _categoryValuationRepo;
    private readonly QuestionRepo _questionRepo;
    private readonly ReferenceRepo _referenceRepo;
    private readonly AnswerRepo _answerRepo;
    private readonly QuestionViewRepository _questionViewRepository;
    private readonly UserActivityRepo _userActivityRepo;
    private readonly QuestionValuationRepo _questionValuationRepo;
    private readonly CommentRepository _commentRepository;
    private readonly ISession _nhibernateSession;
    private readonly UpdateQuestionCountForCategory _updateQuestionCountForCategory;
    private readonly UserRepo _userRepo;

    public DeleteQuestion(CategoryValuationRepo categoryValuationRepo,
        QuestionRepo questionRepo,
        ReferenceRepo referenceRepo,
        AnswerRepo answerRepo,
        QuestionViewRepository questionViewRepository,
        UserActivityRepo userActivityRepo,
        QuestionValuationRepo questionValuationRepo,
        CommentRepository commentRepository,
        ISession nhibernateSession,
        UpdateQuestionCountForCategory updateQuestionCountForCategory,
        UserRepo userRepo)
    {
        _categoryValuationRepo = categoryValuationRepo;
        _questionRepo = questionRepo;
        _referenceRepo = referenceRepo;
        _answerRepo = answerRepo;
        _questionViewRepository = questionViewRepository;
        _userActivityRepo = userActivityRepo;
        _questionValuationRepo = questionValuationRepo;
        _commentRepository = commentRepository;
        _nhibernateSession = nhibernateSession;
        _updateQuestionCountForCategory = updateQuestionCountForCategory;
        _userRepo = userRepo;
    }
    public void Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;
        var questionId = dataMap.GetInt("questionId");
        Logg.r().Information("Job started - DeleteQuestion {id}", questionId);

        SessionUserCache.RemoveQuestionForAllUsers(questionId, _categoryValuationRepo, _userRepo, _questionValuationRepo);

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
        
        var question = _questionRepo.GetById(questionId);

        _questionRepo.Delete(question);

        var categoriesToUpdate = question.Categories.ToList();
        var categoriesToUpdateIds = categoriesToUpdate.Select(c => c.Id).ToList();
        _updateQuestionCountForCategory.Run(categoriesToUpdate);
        JobScheduler.StartImmediately_UpdateAggregatedCategoriesForQuestion(categoriesToUpdateIds);
        Logg.r().Information("Question {id} deleted", questionId);
    }
}