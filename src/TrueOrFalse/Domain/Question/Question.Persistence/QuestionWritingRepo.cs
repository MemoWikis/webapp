using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueOrFalse.Search;

public class QuestionWritingRepo : IRegisterAsInstancePerLifetime
{
    private readonly UpdateQuestionCountForCategory _updateQuestionCountForCategory;
    private readonly JobQueueRepo _jobQueueRepo;
    private readonly ReputationUpdate _reputationUpdate;
    private readonly UserRepo _userRepo;
    private readonly UserActivityRepo _userActivityRepo;
    private readonly QuestionChangeRepo _questionChangeRepo;
    private readonly QuestionRepo _questionRepo;

    public QuestionWritingRepo(UpdateQuestionCountForCategory updateQuestionCountForCategory,
        JobQueueRepo jobQueueRepo,
        ReputationUpdate reputationUpdate,
        UserRepo userRepo,
        UserActivityRepo userActivityRepo,
        QuestionChangeRepo questionChangeRepo,
        QuestionRepo questionRepo) 
    {
        _updateQuestionCountForCategory = updateQuestionCountForCategory;
        _jobQueueRepo = jobQueueRepo;
        _reputationUpdate = reputationUpdate;
        _userRepo = userRepo;
        _userActivityRepo = userActivityRepo;
        _questionChangeRepo = questionChangeRepo;
        _questionRepo = questionRepo;
    }
    public void Create(Question question, CategoryRepository categoryRepository)
    {
        if (question.Creator == null)
        {
            throw new Exception("no creator");
        }

        _questionRepo.Create(question);
        _questionRepo.Flush();

        _updateQuestionCountForCategory.Run(question.Categories);

        foreach (var category in question.Categories.ToList())
        {
            category.UpdateCountQuestionsAggregated(question.Creator.Id);
            categoryRepository.Update(category);
            KnowledgeSummaryUpdate.ScheduleForCategory(category.Id, _jobQueueRepo);
        }

        if (question.Visibility != QuestionVisibility.Owner)
        {
            UserActivityAdd.CreatedQuestion(question, _userRepo, _userActivityRepo);
            _reputationUpdate.ForUser(question.Creator);
        }

        EntityCache.AddOrUpdate(QuestionCacheItem.ToCacheQuestion(question));

        _questionChangeRepo.AddCreateEntry(question);
        Task.Run(async () => await new MeiliSearchQuestionsDatabaseOperations().CreateAsync(question));
    }

    public List<int> Delete(int questionId)
    {
        var question = _questionRepo.GetById(questionId);
        Delete(question);
        var categoriesToUpdate = question.Categories.ToList();
        var categoriesToUpdateIds = categoriesToUpdate.Select(c => c.Id).ToList();
        _updateQuestionCountForCategory.Run(categoriesToUpdate);
        return categoriesToUpdateIds;
    }

    public void Delete(Question question)
    {
        _questionRepo.Delete(question);
        _questionChangeRepo.AddDeleteEntry(question);
        Task.Run(async () => await new MeiliSearchQuestionsDatabaseOperations().DeleteAsync(question));
    }
}
