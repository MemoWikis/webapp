using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Quartz;
using RollbarSharp;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class AddCategoryToWishKnowledge : IJob
    {
        public const int IntervalInSeconds = 2;

        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                var successfullJobIds = new List<int>();
                var jobs = scope.R<JobQueueRepo>().GetAddCategoryToWishKnowledge();
                var categoryUserPairs = new List<CategoryUserPair>();
                foreach (var job in jobs)
                {
                    var serializer = new JavaScriptSerializer();
                    var categoryUserIdPair = serializer.Deserialize<CategoryUserPair>(job.JobContent);
                    categoryUserPairs.Add(categoryUserIdPair);
                }
                foreach (var categoryUserPair in categoryUserPairs)
                {
                    try
                    {
                        var user = Sl.UserRepo.GetById(categoryUserPair.UserId);
                        CategoryInKnowledge.PinQuestionsInCategory(categoryUserPair.CategoryId, user);
                        CategoryInKnowledge.UpdateCategoryValuation(categoryUserPair.CategoryId, user);

                        ////var user = Sl.UserRepo.GetById(categoryUserPair.UserId);
                        ////var questions = Sl.CategoryRepo.GetById(categoryUserPair.CategoryId).GetAggregatedQuestionsFromMemoryCache();
                        ////var questionValuations = Sl.QuestionValuationRepo.GetByQuestionIds(questions.GetIds(), categoryUserPair.UserId);
                        ////foreach (var question in questions)
                        ////{
                        ////    QuestionInKnowledge.CreateOrUpdateValuation(question, questionValuations.ByQuestionId(question.Id), user, relevancePersonal: 50);
                        ////    Sl.Session.CreateSQLQuery(QuestionInKnowledge.GenerateRelevancePersonal(question.Id)).ExecuteUpdate();
                        ////    ProbabilityUpdate_Valuation.Run(question, user);
                        ////}
                        ////QuestionInKnowledge.SetUserWishCountQuestions(user);

                        ////var creatorGroups = questions.Select(q => q.Creator).GroupBy(c => c.Id);

                        ////foreach (var creator in creatorGroups)
                        ////    ReputationUpdate.ForUser(creator.First());

                        //scope.R<ReputationUpdate>().Run(scope.R<UserRepo>().GetById(Convert.ToInt32(userJobs.Key)));
                        //successfullJobIds.AddRange(userJobs.Select(j => j.Id).ToList<int>());
                    }
                    catch (Exception e)
                    {
                        Logg.r().Error(e, "Error in job AddCategoryToWishKnowledge.");
                        new RollbarClient().SendException(e);
                    }
                }

                //Delete jobs that have been executed successfully
                if (successfullJobIds.Count > 0)
                {
                    scope.R<JobQueueRepo>().DeleteById(successfullJobIds);
                    Logg.r().Information("Job AddCategoryToWishKnowledge added for "+ successfullJobIds.Count + " jobs.");
                    successfullJobIds.Clear();
                }

            }, "RecalcReputation");
        }
    }
}
