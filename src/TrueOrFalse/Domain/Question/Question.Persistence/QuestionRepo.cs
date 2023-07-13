using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Criterion;
using Org.BouncyCastle.Bcpg;
using TrueOrFalse.Search;
using TrueOrFalse.Utilities.ScheduledJobs;

public class QuestionRepo : RepositoryDbBase<Question>
{
    private readonly CategoryRepository _categoryRepository;
    private readonly ReputationUpdate _reputationUpdate;
    private readonly JobQueueRepo _jobQueueRepo;
    private readonly UpdateQuestionCountForCategory _updateQuestionCountForCategory;
    private readonly QuestionChangeRepo _questionChangeRepo;
    private readonly QuestionValuationRepo _questionValuationRepo;
    private readonly UserRepo _userRepo;
    private readonly UserActivityRepo _userActivityRepo;


    public QuestionRepo(ISession session,
        CategoryRepository categoryRepository,
        ReputationUpdate reputationUpdate,
        JobQueueRepo jobQueueRepo,
        UpdateQuestionCountForCategory updateQuestionCountForCategory,
        QuestionChangeRepo questionChangeRepo,
        QuestionValuationRepo questionValuationRepo, 
        UserRepo userRepo,
        UserActivityRepo userActivityRepo) : base(session)
    {
        _categoryRepository = categoryRepository;
        _reputationUpdate = reputationUpdate;
        _jobQueueRepo = jobQueueRepo;
        _updateQuestionCountForCategory = updateQuestionCountForCategory;
        _questionChangeRepo = questionChangeRepo;
        _questionValuationRepo = questionValuationRepo;
        _userRepo = userRepo;
        _userActivityRepo = userActivityRepo;
    }

    public override void Create(Question question)
    {
        if (question.Creator == null)
        {
            throw new Exception("no creator");
        }

        base.Create(question);
        Flush();

        _updateQuestionCountForCategory.Run(question.Categories);

        foreach (var category in question.Categories.ToList())
        {
            category.UpdateCountQuestionsAggregated(question.Creator.Id);
            _categoryRepository.Update(category);
            KnowledgeSummaryUpdate.ScheduleForCategory(category.Id, _jobQueueRepo);
        }

        if (question.Visibility != QuestionVisibility.Owner)
        {
            UserActivityAdd.CreatedQuestion(question, _userRepo, _userActivityRepo);
            _reputationUpdate.ForUser(question.Creator);
        }

        EntityCache.AddOrUpdate(QuestionCacheItem.ToCacheQuestion(question));

        _questionChangeRepo.AddCreateEntry(question, this);
        Task.Run(async () => await new MeiliSearchQuestionsDatabaseOperations().CreateAsync(question));
    }

    public override void Delete(Question question)
    {
        base.Delete(question);
        _questionChangeRepo.AddDeleteEntry(question);
        Task.Run(async () => await new MeiliSearchQuestionsDatabaseOperations().DeleteAsync(question));
    }

    public IList<Question> GetAllEager()
    {
        var questions = _session.QueryOver<Question>()
            .Future();

        _session.QueryOver<Question>()
            .Fetch(SelectMode.Fetch, x => x.Categories)
            .Future();

        _session.QueryOver<Question>()
            .Fetch(SelectMode.Fetch, x => x.References)
            .Future();


        var result = questions.ToList();

        foreach (var question in result)
        {
            NHibernateUtil.Initialize(question.Creator);
            NHibernateUtil.Initialize(question.References);
        }

        return result;
    }

    public IList<Question> GetByIds(List<int> questionIds)
    {
        return GetByIds(questionIds.ToArray());
    }

    public override IList<Question> GetByIds(params int[] questionIds)
    {
        var questions = _session
            .QueryOver<Question>()
            .Fetch(q => q.Categories).Eager
            .Where(Restrictions.In("Id", questionIds))
            .List()
            .Distinct()
            .ToList();

        return questionIds
            .Select(questionId => questions.FirstOrDefault(question => question.Id == questionId))
            .Where(q => q != null)
            .ToList();
    }

    public IList<int> GetByKnowledge(
        int userId,
        bool isKnowledgeSolidFilter,
        bool isKnowledgeShouldConsolidateFilter,
        bool isKnowledgeShouldLearnFilter,
        bool isKnowledgeNoneFilter)
    {
        var query = _session
            .QueryOver<QuestionValuation>()
            .Where(q =>
                q.User.Id == userId &&
                q.RelevancePersonal != -1);

        AbstractCriterion knowledgeExpression = null;
        Func<KnowledgeStatus, AbstractCriterion> fnGetCombinedExpression = knowledgeStatus =>
        {
            var knowledgeExpressionToAdd = Restrictions.Eq("KnowledgeStatus", knowledgeStatus);
            return knowledgeExpression != null
                ? Restrictions.Or(knowledgeExpression, knowledgeExpressionToAdd)
                : knowledgeExpressionToAdd;
        };

        if (isKnowledgeSolidFilter)
        {
            knowledgeExpression = fnGetCombinedExpression(KnowledgeStatus.Solid);
        }

        if (isKnowledgeShouldConsolidateFilter)
        {
            knowledgeExpression = fnGetCombinedExpression(KnowledgeStatus.NeedsConsolidation);
        }

        if (isKnowledgeShouldLearnFilter)
        {
            knowledgeExpression = fnGetCombinedExpression(KnowledgeStatus.NeedsLearning);
        }

        if (isKnowledgeNoneFilter)
        {
            knowledgeExpression = fnGetCombinedExpression(KnowledgeStatus.NotLearned);
        }

        if (knowledgeExpression != null)
        {
            query.And(knowledgeExpression);
        }

        return query
            .JoinQueryOver(q => q.Question)
            .Select(q => q.Question.Id)
            .List<int>();
    }

    public IList<Question> GetForCategory(int categoryId)
    {
        return _session.QueryOver<Question>()
            .Where(q => q.Visibility == QuestionVisibility.All)
            .JoinQueryOver<Category>(q => q.Categories)
            .Where(c => c.Id == categoryId)
            .List<Question>();
    }

    public int HowManyNewPublicQuestionsCreatedSince(DateTime since)
    {
        return _session.QueryOver<Question>()
            .Where(q => q.DateCreated > since)
            .And(q => q.Visibility == QuestionVisibility.All)
            .RowCount();
    }

    /// <summary>
    ///     Return how often a question is in other peoples WuWi
    /// </summary>
    public int HowOftenInOtherPeoplesWuwi(int userId, int questionId)
    {
        return _questionValuationRepo
            .Query
            .Where(v =>
                v.User.Id != userId &&
                v.Question.Id == questionId &&
                v.RelevancePersonal > -1
            )
            .RowCount();
    }

    public new void Merge(Question question)
    {
        UpdateOrMerge(question, true);
    }

    public int TotalPublicQuestionCount()
    {
        return _session.QueryOver<Question>()
            .Where(q => q.Visibility == QuestionVisibility.All)
            .RowCount();
    }

    public new void Update(Question question)
    {
        UpdateOrMerge(question, false);
    }

    /// <summary>
    ///     Basic update method, to be used to update only the internally used statistics.
    ///     No cross referencing or validation with other tables is happening here.
    /// </summary>
    public void UpdateFieldsOnly(Question question)
    {
        base.Update(question);
    }

    private void UpdateOrMerge(Question question, bool merge)
    {
        var categoriesIds = _session
            .CreateSQLQuery("SELECT Category_id FROM categories_to_questions WHERE Question_id =" + question.Id)
            .List<int>();
        var query = "SELECT Category_id FROM reference WHERE Question_id=" + question.Id +
                    " AND Category_id is not null";
        var categoriesReferences = _session
            .CreateSQLQuery(query)
            .List<int>();
        var categoriesBeforeUpdateIds = categoriesIds.Union(categoriesReferences)
            .ToList();

        var categoriesBeforeUpdate =_session
            .QueryOver<Category>()
            .WhereRestrictionOn(c => c.Id)
            .IsIn(categoriesBeforeUpdateIds)
            .List();


        if (merge)
        {
            base.Merge(question);
        }
        else
        {
            base.Update(question);
        }

        Flush();

        var categoriesToUpdate = categoriesBeforeUpdate
            .Union(question.Categories)
            .Union(question.References.Where(r => r.Category != null)
                .Select(r => r.Category))
            .ToList();

        var categoriesToUpdateIds = categoriesToUpdate.Select(c => c.Id).ToList();
        EntityCache.AddOrUpdate(QuestionCacheItem.ToCacheQuestion(question), categoriesToUpdateIds);
        _updateQuestionCountForCategory.Run(categoriesToUpdateIds);
        JobScheduler.StartImmediately_UpdateAggregatedCategoriesForQuestion(categoriesToUpdateIds);
        _questionChangeRepo.AddUpdateEntry(question, this);

        Task.Run(async () => await new MeiliSearchQuestionsDatabaseOperations().UpdateAsync(question));
    }
}