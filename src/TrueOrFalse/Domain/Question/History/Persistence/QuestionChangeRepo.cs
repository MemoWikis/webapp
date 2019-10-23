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

    public void AddCreateEntry(Question question, User author = null) => AddUpdateOrCreateEntry(question, QuestionChangeType.Create, author, imageWasChanged:true);
    public void AddUpdateEntry(Question question, User author = null, bool imageWasChanged = false) => AddUpdateOrCreateEntry(question, QuestionChangeType.Update, author, imageWasChanged);
    private void AddUpdateOrCreateEntry(Question question, QuestionChangeType questionChangeType, User author, bool imageWasChanged)
    {
        if (author == null)
            author = Sl.SessionUser.User;

        var questionChange = new QuestionChange
        {
            Question = question,
            Type = questionChangeType,
            Author = author,
            DataVersion = 1
        };

        questionChange.SetData(question, imageWasChanged);

        base.Create(questionChange);
    }

    public void CreateQuestionChange(Question question)
    {
        var questionChange = new QuestionChange
        {
            Question = question,
            Type = QuestionChangeType.Create,
            Author = question.Creator,
            DataVersion = 1
        };

        questionChange.SetData(question,true);

        base.Create(questionChange);
    }
    public void AddUpdateOrCreateEntryWithoutSession(Question question,int changeQuestionId)
    {
        if (question.Creator != null)
            Session
                .CreateSQLQuery(
                    "Update questionChange Set Author_Id = :userId, DateCreated = :dateCreated Where Id =  :questionId")
                .SetParameter("userId", question.Creator.Id).SetParameter("questionId", changeQuestionId)
                .SetParameter("dateCreated", question.DateCreated).ExecuteUpdate();
        else
            Session
                .CreateSQLQuery(
                    "Update questionChange Set Author_Id = null, DateCreated = :dateCreated Where Id =  :questionId")
                .SetParameter("questionId", changeQuestionId).SetParameter("dateCreated", question.DateCreated)
                .ExecuteUpdate();
    }

    public IList<QuestionChange> GetAllEager()
    {
        return _session
            .QueryOver<QuestionChange>()
            .Left.JoinQueryOver(q => q.Question)
            .List();
    }

    public IList<QuestionChange> GetForQuestion(int questionId, bool filterUsersForSidebar = false)
    {
        User aliasUser = null;
        Question aliasQuestion = null;

        var query = _session
            .QueryOver<QuestionChange>()
            .Where(c => c.Question.Id == questionId);

        if (filterUsersForSidebar)
            query.And(c => c.ShowInSidebar);

        query.Left.JoinAlias(c => c.Author, () => aliasUser)
            .Left.JoinAlias(c => c.Question, () => aliasQuestion);

        return query.List();
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