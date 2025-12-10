using NHibernate;
using NHibernate.Linq;


public class QuestionValuationReadingRepo : RepositoryDb<QuestionValuation>
{
    public QuestionValuationReadingRepo(ISession session) : base(session)
    {
       
    }

    public int HowOftenInOtherPeoplesWishKnowledge(int userId, int questionId)
    {
        return _session
            .QueryOver<QuestionValuation>()
            .Where(v =>
                v.User.Id != userId &&
                v.Question.Id == questionId &&
                v.RelevancePersonal > -1
            )
            .RowCount();
    }

    public QuestionValuation? GetBy(int questionId, int userId)
    {
        return _session.QueryOver<QuestionValuation>()
            .Where(q =>
                q.User.Id == userId &&
                q.Question.Id == questionId)
            .SingleOrDefault();
    }

    public IList<QuestionValuation> GetByQuestionIds(IEnumerable<int> questionIds, int userId)
    {
        return
            _session.QueryOver<QuestionValuation>()
                .WhereRestrictionOn(x => x.Question.Id).IsIn(questionIds.ToArray())
                .And(x => x.User.Id == userId)
                .List<QuestionValuation>();
    }

    public IList<QuestionValuation> GetByUser(int userId, bool onlyActiveKnowledge = true)
    {
        var query = _session
            .QueryOver<QuestionValuation>()
            .Where(q => q.User.Id == userId);

        if (onlyActiveKnowledge)
        {
            query.And(q => q.RelevancePersonal > -1);
        }

        return query.List<QuestionValuation>();
    }

    public IList<QuestionValuation> GetByUserWithQuestion(int userId)
    {
        var result = _session
            .Query<QuestionValuation>()
            .Where(qv => qv.User.Id == userId)
            .Fetch(qv => qv.Question)
            .ThenFetchMany(q => q.Pages)
            .ToList();

        return result;
    }
}