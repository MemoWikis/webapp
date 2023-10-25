using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;
using TrueOrFalse.Utilities.ScheduledJobs;
using ISession = NHibernate.ISession;

public class QuestionWritingRepo : RepositoryDbBase<Question>
{
    private readonly UpdateQuestionCountForCategory _updateQuestionCountForCategory;
    private readonly JobQueueRepo _jobQueueRepo;
    private readonly ReputationUpdate _reputationUpdate;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly UserActivityRepo _userActivityRepo;
    private readonly QuestionChangeRepo _questionChangeRepo;
    private readonly ISession _nhibernateSession;
    private readonly RepositoryDb<Question> _repo;


    public QuestionWritingRepo(UpdateQuestionCountForCategory updateQuestionCountForCategory,
        JobQueueRepo jobQueueRepo,
        ReputationUpdate reputationUpdate,
        UserReadingRepo userReadingRepo,
        UserActivityRepo userActivityRepo,
        QuestionChangeRepo questionChangeRepo,
        ISession nhibernateSession) : base(nhibernateSession)
    {
        _repo = new RepositoryDb<Question>(nhibernateSession);
        _updateQuestionCountForCategory = updateQuestionCountForCategory;
        _jobQueueRepo = jobQueueRepo;
        _reputationUpdate = reputationUpdate;
        _userReadingRepo = userReadingRepo;
        _userActivityRepo = userActivityRepo;
        _questionChangeRepo = questionChangeRepo;
        _nhibernateSession = nhibernateSession;
    }
    public void Create(Question question, CategoryRepository categoryRepository)
    {
        if (question.Creator == null)
        {
            throw new Exception("no creator");
        }

        _repo.Create(question);
        _repo.Flush();

        _updateQuestionCountForCategory.Run(question.Categories);

        foreach (var category in question.Categories.ToList())
        {
            category.UpdateCountQuestionsAggregated(question.Creator.Id);
            categoryRepository.Update(category);
            KnowledgeSummaryUpdate.ScheduleForCategory(category.Id, _jobQueueRepo);
        }

        if (question.Visibility != QuestionVisibility.Owner)
        {
            UserActivityAdd.CreatedQuestion(question, _userReadingRepo, _userActivityRepo);
            _reputationUpdate.ForUser(question.Creator);
        }

        EntityCache.AddOrUpdate(QuestionCacheItem.ToCacheQuestion(question));

        _questionChangeRepo.AddCreateEntry(question);
        Task.Run(async () => await new MeiliSearchQuestionsDatabaseOperations()
            .CreateAsync(question));
    }

    public List<int> Delete(int questionId)
    {
        var question = GetById(questionId);
        Delete(question);
        var categoriesToUpdate = question.Categories.ToList();
        var categoriesToUpdateIds = categoriesToUpdate.Select(c => c.Id).ToList();
        _updateQuestionCountForCategory.Run(categoriesToUpdate);
        return categoriesToUpdateIds;
    }

    public void Delete(Question question)
    {
        base.Delete(question);
        _questionChangeRepo.AddDeleteEntry(question);
        Task.Run(async () => await new MeiliSearchQuestionsDatabaseOperations()
            .DeleteAsync(question));
    }

    public  void UpdateOrMerge(Question question, bool withMerge)
    {
        var categoriesIds = _nhibernateSession
            .CreateSQLQuery("SELECT Category_id FROM categories_to_questions WHERE Question_id =" + question.Id)
            .List<int>();
        var query = "SELECT Category_id FROM reference WHERE Question_id=" + question.Id +
                    " AND Category_id is not null";
        var categoriesReferences = _nhibernateSession
            .CreateSQLQuery(query)
            .List<int>();
        var categoriesBeforeUpdateIds = categoriesIds.Union(categoriesReferences)
            .ToList();

        var categoriesBeforeUpdate = _nhibernateSession
            .QueryOver<Category>()
            .WhereRestrictionOn(c => c.Id)
            .IsIn(categoriesBeforeUpdateIds)
            .List();

        UpdateOrMerge(question, withMerge);
        if (withMerge)
        {
            Merge(question);
        }
        else
        {
          Update(question);
        }
        Flush();
        var categoriesToUpdate = categoriesBeforeUpdate
            .Union(question.Categories)
            .Union(question.References.Where(r => r.Category != null)
                .Select(r => r.Category))
            .ToList();

        var categoriesToUpdateIds = categoriesToUpdate.Select(c => c.Id).ToList();
        EntityCache.AddOrUpdate(QuestionCacheItem.ToCacheQuestion(question), categoriesToUpdateIds);
        _updateQuestionCountForCategory.Run(categoriesToUpdate);
        JobScheduler.StartImmediately_UpdateAggregatedCategoriesForQuestion(categoriesToUpdateIds);
        _questionChangeRepo.AddUpdateEntry(question);

        Task.Run(async () => await new MeiliSearchQuestionsDatabaseOperations()
            .UpdateAsync(question));
    }
    public void UpdateFieldsOnly(Question question)
    {
        base.Update(question);
    }
}
