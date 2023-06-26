using NHibernate;

public class QuestionChangeRepo : RepositoryDbBase<QuestionChange>,IRegisterAsInstancePerLifetime
{
    private readonly SessionUser _sessionUser;
    public QuestionChangeRepo(ISession session, SessionUser sessionUser) : base(session)
    {
        _sessionUser = sessionUser;
    }

    public void AddDeleteEntry(Question question)
    {
        var QuestionChange = new QuestionChange
        {
            Question = question,
            AuthorId = _sessionUser.IsLoggedIn ? _sessionUser.UserId : default,
            Type = QuestionChangeType.Delete,
            DataVersion = 1
        };

        base.Create(QuestionChange);
    }

    public void AddCreateEntry(Question question) => AddUpdateOrCreateEntry(question, QuestionChangeType.Create, question.Creator, imageWasChanged:true);
    public void AddUpdateEntry(Question question, User author = null, bool imageWasChanged = false) => AddUpdateOrCreateEntry(question, QuestionChangeType.Update, author, imageWasChanged);
    private void AddUpdateOrCreateEntry(Question question, QuestionChangeType questionChangeType, User author, bool imageWasChanged)
    {
        var questionChange = new QuestionChange
        {
            Question = question,
            Type = questionChangeType,
            AuthorId = author != null ? author.Id : _sessionUser.IsLoggedIn ? _sessionUser.UserId : default,
            DataVersion = 1
        };

        questionChange.SetData(question, imageWasChanged);

        base.Create(questionChange);
    }

    public void Create(Question question)
    {
        var questionChange = new QuestionChange
        {
            Question = question,
            Type = QuestionChangeType.Create,
            AuthorId = question.Creator == null ? -1 : question.Creator.Id,
            DataVersion = 1
        };

        questionChange.SetData(question, true);

        base.Create(questionChange);
    }

    public QuestionChange GetByIdEager(int questionChangeId)
    {
        return _session
            .QueryOver<QuestionChange>()
            .Where(cc => cc.Id == questionChangeId)
            .Left.JoinQueryOver(q => q.Question)
            .SingleOrDefault();
    }
}