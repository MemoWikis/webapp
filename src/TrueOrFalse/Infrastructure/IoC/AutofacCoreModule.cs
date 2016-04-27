using System;
using System.IO;
using System.Reflection;
using System.Text;
using Autofac;
using NHibernate;
using TrueOrFalse.Infrastructure.Persistence;
using TrueOrFalse.Maintenance;
using TrueOrFalse.Search;
using TrueOrFalse.Updates;
using TrueOrFalse.Utilities.ScheduledJobs;

namespace TrueOrFalse.Infrastructure
{
    public class AutofacCoreModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterTypes(builder);

            builder.RegisterType<CleanUpWorkInProgressQuestions>().InstancePerLifetimeScope();
            builder.RegisterType<GameLoop>().InstancePerLifetimeScope();
            builder.RegisterType<RecalcKnowledgeStati>().InstancePerLifetimeScope();
            builder.RegisterType<TrainingReminderCheck>().InstancePerLifetimeScope();
            builder.RegisterType<TrainingPlanUpdateCheck>().InstancePerLifetimeScope();

            try
            {
                builder.RegisterInstance(SessionFactory.CreateSessionFactory());
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();

                var innerException = ex.InnerException;
                while (innerException != null)
                {
                    if (innerException is ReflectionTypeLoadException)
                        break;

                    innerException = innerException.InnerException;
                }

                if (innerException == null)
                    throw;

                foreach (Exception exSub in ((ReflectionTypeLoadException)innerException).LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    if (exSub is FileNotFoundException)
                    {
                        var exFileNotFound = exSub as FileNotFoundException;
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }

                throw new Exception(sb.ToString());
            }

            builder.Register(context => new SessionManager(context.Resolve<ISessionFactory>().OpenSession())).InstancePerLifetimeScope();
            builder.Register(context => context.Resolve<SessionManager>().Session).ExternallyOwned();
        }

        private void RegisterTypes(ContainerBuilder builder)
        {
            //GENERATED WITH AutofacRegistrationWriter
            //in order to improve performance
            builder.RegisterType<CategoryDeleter>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryExists>().InstancePerLifetimeScope();
            builder.RegisterType<UpdateSetCountForCategory>().InstancePerLifetimeScope();
            builder.RegisterType<CommentRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DeleteDate>().InstancePerLifetimeScope();
            builder.RegisterType<CopyDate>().InstancePerLifetimeScope();
            builder.RegisterType<GetDatesInNetwork>().InstancePerLifetimeScope();
            builder.RegisterType<DateRepo>().InstancePerLifetimeScope();
            builder.RegisterType<GameCreate>().InstancePerLifetimeScope();
            builder.RegisterType<GameStatusChange>().InstancePerLifetimeScope();
            builder.RegisterType<RoundRepo>().InstancePerLifetimeScope();
            builder.RegisterType<PlayerRepo>().InstancePerLifetimeScope();
            builder.RegisterType<GameHubConnection>().InstancePerLifetimeScope();
            builder.RegisterType<AddRoundsToGame>().InstancePerLifetimeScope();
            builder.RegisterType<IsCurrentlyIn>().InstancePerLifetimeScope();
            builder.RegisterType<GameRepo>().InstancePerLifetimeScope();
            builder.RegisterType<AnswerPatternRepo>().InstancePerLifetimeScope();
            builder.RegisterType<TrainingDateRepo>().InstancePerLifetimeScope();
            builder.RegisterType<TrainingPlanRepo>().InstancePerLifetimeScope();
            builder.RegisterType<LearningSessionStepRepo>().InstancePerLifetimeScope();
            builder.RegisterType<AlgoInfoRepo>().InstancePerLifetimeScope();
            builder.RegisterType<AnswerTestRepo>().InstancePerLifetimeScope();
            builder.RegisterType<AnswerFeatureRepo>().InstancePerLifetimeScope();
            builder.RegisterType<QuestionFeatureRepo>().InstancePerLifetimeScope();
            builder.RegisterType<ProbabilityCalc_Question>().InstancePerLifetimeScope();
            builder.RegisterType<ProbabilityUpdate_Question>().InstancePerLifetimeScope();
            builder.RegisterType<GetWishSetCount>().InstancePerLifetimeScope();
            builder.RegisterType<AddValuationEntries_ForQuestionsInSetsAndDates>().InstancePerLifetimeScope();
            builder.RegisterType<ProbabilityCalc_Simple3>().InstancePerLifetimeScope();
            builder.RegisterType<ProbabilityCalc_Simple2>().InstancePerLifetimeScope();
            builder.RegisterType<KnowledgeSummaryLoader>().InstancePerLifetimeScope();
            builder.RegisterType<LearningSession>().InstancePerLifetimeScope();
            builder.RegisterType<LearningSessionStep>().InstancePerLifetimeScope();
            builder.RegisterType<LearningSessionRepo>().InstancePerLifetimeScope();
            builder.RegisterType<UpdateImageTypes>().InstancePerLifetimeScope();
            builder.RegisterType<LicenseRepo>().InstancePerLifetimeScope();
            builder.RegisterType<GetUnreadMessageCount>().InstancePerLifetimeScope();
            builder.RegisterType<SetMessageRead>().InstancePerLifetimeScope();
            builder.RegisterType<MessageRepo>().InstancePerLifetimeScope();
            builder.RegisterType<SetMessageUnread>().InstancePerLifetimeScope();
            builder.RegisterType<GetTotalSetCount>().InstancePerLifetimeScope();
            builder.RegisterType<UpdateQuestionCountForCategory>().InstancePerLifetimeScope();
            builder.RegisterType<GetStreaksDays>().InstancePerLifetimeScope();
            builder.RegisterType<UserActivityRepo>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ImageStore>().InstancePerLifetimeScope();
            builder.RegisterType<ImageMetaDataRepo>().InstancePerLifetimeScope();
            builder.RegisterType<ProbabilityCalc_Simple1>().InstancePerLifetimeScope();
            builder.RegisterType<GetWishQuestionCountCached>().InstancePerLifetimeScope();
            builder.RegisterType<GetWishQuestionCount>().InstancePerLifetimeScope();
            builder.RegisterType<KnowledgeHistoryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AddToSet>().InstancePerLifetimeScope();
            builder.RegisterType<GetTotalCategories>().InstancePerLifetimeScope();
            builder.RegisterType<SetDeleter>().InstancePerLifetimeScope();
            builder.RegisterType<UpdateQuestionsOrder>().InstancePerLifetimeScope();
            builder.RegisterType<QuestionInSetRepo>().InstancePerLifetimeScope();
            builder.RegisterType<SetRepo>().InstancePerLifetimeScope();
            builder.RegisterType<CreateOrUpdateSetValue>().InstancePerLifetimeScope();
            builder.RegisterType<DeleteValuationsForNonExisitingSets>().InstancePerLifetimeScope();
            builder.RegisterType<GetSetTotal>().InstancePerLifetimeScope();
            builder.RegisterType<SetValuationRepo>().InstancePerLifetimeScope();
            builder.RegisterType<GetAnswerStatsInPeriod>().InstancePerLifetimeScope();
            builder.RegisterType<TotalsPersUserLoader>().InstancePerLifetimeScope();
            builder.RegisterType<UpdateQuestionAnswerCounts>().InstancePerLifetimeScope();
            builder.RegisterType<UpdateQuestionAnswerCount>().InstancePerLifetimeScope();
            builder.RegisterType<ReferenceRepo>().InstancePerLifetimeScope();
            builder.RegisterType<GetQuestionSolution>().InstancePerLifetimeScope();
            builder.RegisterType<UpdateSetDataForQuestion>().InstancePerLifetimeScope();
            builder.RegisterType<QuestionGetCount>().InstancePerLifetimeScope();
            builder.RegisterType<QuestionSearchSpecSession>().InstancePerLifetimeScope();
            builder.RegisterType<CreateOrUpdateQuestionValue>().InstancePerLifetimeScope();
            builder.RegisterType<GetQuestionTotal>().InstancePerLifetimeScope();
            builder.RegisterType<QuestionValuationRepo>().InstancePerLifetimeScope();
            builder.RegisterType<SaveQuestionView>().InstancePerLifetimeScope();
            builder.RegisterType<QuestionViewRepository>().InstancePerLifetimeScope();
            builder.RegisterType<BadgeRepo>().InstancePerLifetimeScope();
            builder.RegisterType<ReputationCalc>().InstancePerLifetimeScope();
            builder.RegisterType<ReputationUpdate>().InstancePerLifetimeScope();
            builder.RegisterType<UpdateReputationsAndRank>().InstancePerLifetimeScope();
            builder.RegisterType<AppAccessRepo>().InstancePerLifetimeScope();
            builder.RegisterType<InvoiceCreate>().InstancePerLifetimeScope();
            builder.RegisterType<FollowerIAm>().InstancePerLifetimeScope();
            builder.RegisterType<FollowerCounts>().InstancePerLifetimeScope();
            builder.RegisterType<GetTotalUsers>().InstancePerLifetimeScope();
            builder.RegisterType<TotalIFollow>().InstancePerLifetimeScope();
            builder.RegisterType<UpdateWishcount>().InstancePerLifetimeScope();
            builder.RegisterType<MembershipRepo>().InstancePerLifetimeScope();
            builder.RegisterType<PasswordRecovery>().InstancePerLifetimeScope();
            builder.RegisterType<PasswordReset>().InstancePerLifetimeScope();
            builder.RegisterType<PasswordRecoveryTokenRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UserSummary>().InstancePerLifetimeScope();
            builder.RegisterType<TotalFollowers>().InstancePerLifetimeScope();
            builder.RegisterType<AnswerLog>().InstancePerLifetimeScope();
            builder.RegisterType<AnswerRepo>().InstancePerLifetimeScope();
            builder.RegisterType<AnswerQuestion>().InstancePerLifetimeScope();
            builder.RegisterType<QuestionRepo>().InstancePerLifetimeScope();
            builder.RegisterType<CredentialsAreValid>().InstancePerLifetimeScope();
            builder.RegisterType<ValidateEmailConfirmationKey>().InstancePerLifetimeScope();
            builder.RegisterType<RegisterUser>().InstancePerLifetimeScope();
            builder.RegisterType<UserRepo>().InstancePerLifetimeScope();
            builder.RegisterType<PersistentLoginRepo>().InstancePerLifetimeScope();
            builder.RegisterType<SessionUiData>().InstancePerLifetimeScope();
            builder.RegisterType<SessionUser>().InstancePerLifetimeScope();
            builder.RegisterType<GetImageMaintenanceInfos>().InstancePerLifetimeScope();
            builder.RegisterType<WikiImageLicenseLoader>().InstancePerLifetimeScope();
            builder.RegisterType<WikiImageMetaLoader>().InstancePerLifetimeScope();
            builder.RegisterType<ProbabilityUpdate_ValuationAll>().InstancePerLifetimeScope();
            builder.RegisterType<ProbabilityUpdate_Valuation>().InstancePerLifetimeScope();
            builder.RegisterType<Importer>().InstancePerLifetimeScope();
            builder.RegisterType<SampleData>().InstancePerLifetimeScope();
            builder.RegisterType<UpdateStepExecuter>().InstancePerLifetimeScope();
            builder.RegisterType<Update>().InstancePerLifetimeScope();
            builder.RegisterType<ReIndexAllCategories>().InstancePerLifetimeScope();
            builder.RegisterType<SearchIndexCategory>().InstancePerLifetimeScope();
            builder.RegisterType<SearchCategories>().InstancePerLifetimeScope();
            builder.RegisterType<ReIndexAllSets>().InstancePerLifetimeScope();
            builder.RegisterType<SearchIndexSet>().InstancePerLifetimeScope();
            builder.RegisterType<SearchSets>().InstancePerLifetimeScope();
            builder.RegisterType<SearchIndexQuestion>().InstancePerLifetimeScope();
            builder.RegisterType<ReIndexAllQuestions>().InstancePerLifetimeScope();
            builder.RegisterType<SearchQuestions>().InstancePerLifetimeScope();
            builder.RegisterType<ReIndexAllUsers>().InstancePerLifetimeScope();
            builder.RegisterType<SearchIndexUser>().InstancePerLifetimeScope();
            builder.RegisterType<SearchUsers>().InstancePerLifetimeScope();
            builder.RegisterType<DbSettingsRepo>().InstancePerLifetimeScope();
            builder.RegisterType<DoesTableExist>().InstancePerLifetimeScope();
            builder.RegisterType<ParseMarkupFromDb>().InstancePerLifetimeScope();
            builder.RegisterType<LoadImageMarkups>().InstancePerLifetimeScope();
        }
    }
}