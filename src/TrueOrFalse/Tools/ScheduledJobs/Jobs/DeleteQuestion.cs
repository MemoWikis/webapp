using System.Linq;
using NHibernate;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs;

public class DeleteQuestion : IJob
{
    private readonly CategoryValuationRepo _categoryValuationRepo;

    public DeleteQuestion(CategoryValuationRepo categoryValuationRepo)
    {
        _categoryValuationRepo = categoryValuationRepo;
    }
    public void Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;
        var questionId = dataMap.GetInt("questionId");
        Logg.r().Information("Job started - DeleteQuestion {id}", questionId);

        SessionUserCache.RemoveQuestionForAllUsers(questionId, _categoryValuationRepo);

        //delete connected db-entries
        Sl.R<ReferenceRepo>().DeleteForQuestion(questionId);
        Sl.R<AnswerRepo>().DeleteFor(questionId);
        Sl.R<QuestionViewRepository>().DeleteForQuestion(questionId);
        Sl.R<UserActivityRepo>().DeleteForQuestion(questionId);
        Sl.R<QuestionViewRepository>().DeleteForQuestion(questionId);
        Sl.R<QuestionValuationRepo>().DeleteForQuestion(questionId);
        Sl.R<CommentRepository>().DeleteForQuestion(questionId);
        Sl.R<ISession>()
            .CreateSQLQuery("DELETE FROM categories_to_questions where Question_id = " + questionId)
            .ExecuteUpdate();
        var questionRepo = Sl.QuestionRepo;
        var question = questionRepo.GetById(questionId);

        questionRepo.Delete(question);

        var categoriesToUpdateIds = question.Categories.Select(c => c.Id).ToList();

        Sl.R<UpdateQuestionCountForCategory>().Run(categoriesToUpdateIds);
        JobScheduler.StartImmediately_UpdateAggregatedCategoriesForQuestion(categoriesToUpdateIds);
        Logg.r().Information("Question {id} deleted", questionId);
    }
}