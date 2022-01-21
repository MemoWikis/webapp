using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Data;
using NHibernate;
using NHibernate.Mapping;
using Quartz;


namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class DeleteQuestion : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Logg.r().Information("Job started - DeleteQuestion");

            var dataMap = context.JobDetail.JobDataMap;
            var questionId = dataMap.GetInt("questionId");

            UserCache.RemoveQuestionForAllUsers(questionId);

            //delete connected db-entries
            Sl.R<ReferenceRepo>().DeleteForQuestion(questionId);
            Sl.R<AnswerRepo>().DeleteFor(questionId); //not accounted for: answerfeature_to_answer
            Sl.R<QuestionViewRepository>().DeleteForQuestion(questionId);
            Sl.R<UserActivityRepo>().DeleteForQuestion(questionId);
            Sl.R<QuestionViewRepository>().DeleteForQuestion(questionId);
            Sl.R<QuestionValuationRepo>().DeleteForQuestion(questionId);
            Sl.R<CommentRepository>().DeleteForQuestion(questionId);
            Sl.R<ISession>()
                .CreateSQLQuery("DELETE FROM categories_to_questions where Question_id = " + questionId)
                .ExecuteUpdate();
            Sl.R<ISession>()
                .CreateSQLQuery("DELETE FROM questionFeature_to_question where Question_id = " + questionId)
                .ExecuteUpdate(); //probably not necessary

            var questionRepo = Sl.QuestionRepo;
            var question = questionRepo.GetById(questionId);

            questionRepo.Delete(question);
            Logg.r().Information("Delete Question {id}", questionId);

            var categoriesToUpdateIds = question.Categories.Select(c => c.Id).ToList();

            Sl.R<UpdateQuestionCountForCategory>().Run(categoriesToUpdateIds);
            JobScheduler.StartImmediately_UpdateAggregatedCategoriesForQuestion(categoriesToUpdateIds);
        }
    }
}