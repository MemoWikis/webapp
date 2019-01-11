using System.Collections.Generic;
using NHibernate;


public class QuestionChangeParameters
{

}

public class QuestionChangeRepo : RepositoryDbBase<QuestionChange>
{
    public QuestionChangeRepo(ISession session) : base(session){}

    public void AddDeleteEntry(Question question)
    {
        var QuestionChange = new QuestionChange
        {
            Question = question,
            Author = Sl.SessionUser.User,
            Type = QuestionChangeType.Delete,
            DataVersion = 1
        };

        base.Create(QuestionChange);
    }

    public void AddCreateEntry(Question question) => AddUpdateOrCreateEntry(question, QuestionChangeType.Create);
    public void AddUpdateEntry(Question question) => AddUpdateOrCreateEntry(question, QuestionChangeType.Update);

    private void AddUpdateOrCreateEntry(Question question, QuestionChangeType questionChangeType)
    {
        var QuestionChange = new QuestionChange
        {
            Question = question,
            Type = questionChangeType,
            Author = Sl.SessionUser.User,
            DataVersion = 1
        };
        
        QuestionChange.SetData(question);

        base.Create(QuestionChange);
    }

    public IList<QuestionChange> GetAllEager()
    {
        return _session
            .QueryOver<QuestionChange>()
            .Left.JoinQueryOver(q => q.Question)
            .List();
    }

    public IList<QuestionChange> GetForQuestion(int questionId)
    {
        User aliasUser = null;
        Question aliasQuestion = null;

        var query = _session
            .QueryOver<QuestionChange>()
            .Where(c => c.Question.Id == questionId)
            .JoinAlias(c => c.Author, () => aliasUser)
            .JoinAlias(c => c.Question, () => aliasQuestion);

        return query
            .List();
    }

    public QuestionChange GetByIdEager(int questionChangeId)
    {
        return _session
            .QueryOver<QuestionChange>()
            .Where(cc => cc.Id == questionChangeId)
            .Left.JoinQueryOver(q => q.Question)
            .SingleOrDefault();
    }

    public virtual QuestionChange GetNextRevision(QuestionChange questionChange)
    {
        var questionId = questionChange.Question.Id;
        var currentRevisionDate = questionChange.DateCreated.ToString("yyyy-MM-dd HH-mm-ss");
        var query = $@"

            SELECT * FROM QuestionChange qc
            WHERE qc.Question_Id = {questionId} and qc.DateCreated > '{currentRevisionDate}' 
            ORDER BY qc.DateCreated 
            LIMIT 1

            ";
        var nextRevision = Sl.R<ISession>().CreateSQLQuery(query).AddEntity(typeof(QuestionChange)).UniqueResult<QuestionChange>();
        return nextRevision;
    }

    public virtual QuestionChange GetPreviousRevision(QuestionChange questionChange)
    {
        var questionId = questionChange.Question.Id;
        var currentRevisionDate = questionChange.DateCreated.ToString("yyyy-MM-dd HH-mm-ss");
        var query = $@"

            SELECT * FROM QuestionChange qc
            WHERE qc.Question_Id = {questionId} and qc.DateCreated < '{currentRevisionDate}' 
            ORDER BY qc.DateCreated DESC 
            LIMIT 1

            ";
        var previousRevision = Sl.R<ISession>().CreateSQLQuery(query).AddEntity(typeof(QuestionChange)).UniqueResult<QuestionChange>();
        return previousRevision;
    }
}