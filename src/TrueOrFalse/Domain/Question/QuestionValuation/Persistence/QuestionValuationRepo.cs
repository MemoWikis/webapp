using System.Collections.Generic;
using System.Linq;
using NHibernate;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;

public class QuestionValuationRepo : RepositoryDb<QuestionValuation> 
{
    private readonly SearchIndexQuestion _searchIndexQuestion;
    private readonly QuestionRepository _questionRepository;

    public QuestionValuationRepo(
        ISession session, 
        SearchIndexQuestion searchIndexQuestion,
        QuestionRepository questionRepository) : base(session)
    {
        _searchIndexQuestion = searchIndexQuestion;
        _questionRepository = questionRepository;
    }

    public QuestionValuation GetBy(int questionId, int userId)
    {
        return 
            _session.QueryOver<QuestionValuation>()
                    .Where(q => 
                        q.User.Id == userId && 
                        q.Question.Id == questionId)
                    .SingleOrDefault();
    }

    public IList<QuestionValuation> GetActiveInWishknowledge(int questionId)
    {
        return 
            _session.QueryOver<QuestionValuation>()
                    .Where(q => 
                        q.Question.Id == questionId &&
                        q.RelevancePersonal > -1)
                    .List<QuestionValuation>();
    }

    public IList<QuestionValuation> GetByQuestionIds(IEnumerable<int> questionIds)
    {
        return
            _session.QueryOver<QuestionValuation>()
                    .WhereRestrictionOn(x => x.Question.Id).IsIn(questionIds.ToArray())
                    .List<QuestionValuation>();        
    }

    public IList<QuestionValuation> GetByUser(User user, bool onlyActiveKnowledge = true)
    {
        return GetByUser(user.Id);
    }

    public IList<QuestionValuation> GetByUser(int userId, bool onlyActiveKnowledge = true)
    {
        var query = _session
            .QueryOver<QuestionValuation>()
            .Where(q => q.User.Id == userId);

        if (onlyActiveKnowledge)
            query.And(q => q.RelevancePersonal > -1);

        return query.List<QuestionValuation>();
    }

    public IList<QuestionValuation> GetActiveInWishknowledge(IList<int> questionIds, int userId)
    {
        if(!questionIds.Any())
            return new List<QuestionValuation>();

        return _session.QueryOver<QuestionValuation>()
            .Where(qv => qv.User.Id == userId)
            .AndRestrictionOn(qv => qv.Question.Id).IsIn(questionIds.ToArray())
            .List<QuestionValuation>();
    }

    public override void Create(IList<QuestionValuation> questionValuations)
    {
        base.Create(questionValuations);
        _searchIndexQuestion.Update(_questionRepository.GetByIds(questionValuations.QuestionIds().ToArray()));
    }

    public override void Create(QuestionValuation questionValuation)
    {
        base.Create(questionValuation);
        _searchIndexQuestion.Update(_questionRepository.GetById(questionValuation.Question.Id));
    }

    public override void CreateOrUpdate(QuestionValuation questionValuation)
    {
        base.CreateOrUpdate(questionValuation);
        _searchIndexQuestion.Update(_questionRepository.GetById(questionValuation.Question.Id));
    }

    public override void Update(QuestionValuation questionValuation)
    {
        base.Update(questionValuation);
        _searchIndexQuestion.Update(_questionRepository.GetById(questionValuation.Question.Id));
    }
}