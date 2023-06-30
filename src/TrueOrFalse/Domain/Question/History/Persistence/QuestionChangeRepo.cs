﻿using NHibernate;

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

    public virtual void SetData(Question question, bool imageWasChanged, QuestionChange questionChange, QuestionRepo questionRepo)
    {
        switch (questionChange.DataVersion)
        {
            case 1:
                questionChange.Data = new QuestionEditData_V1(question, imageWasChanged, _session, questionRepo).ToJson();
                break;

            default:
                throw new ArgumentOutOfRangeException($"Invalid data version number {questionChange.DataVersion} for question change id {questionChange.Id}");
        }
    }

    public void AddCreateEntry(Question question, QuestionRepo questionRepo) => AddUpdateOrCreateEntry(question, QuestionChangeType.Create, question.Creator, imageWasChanged:true, questionRepo);
    public void AddUpdateEntry(Question question, QuestionRepo questionRepo, User author = null, bool imageWasChanged = false) => AddUpdateOrCreateEntry(question, QuestionChangeType.Update, author, imageWasChanged, questionRepo);
    private void AddUpdateOrCreateEntry(Question question,
        QuestionChangeType questionChangeType,
        User author,
        bool imageWasChanged,
        QuestionRepo questionRepo)
    {
        var questionChange = new QuestionChange
        {
            Question = question,
            Type = questionChangeType,
            AuthorId = author != null ? author.Id : _sessionUser.IsLoggedIn ? _sessionUser.UserId : default,
            DataVersion = 1
        };

        SetData(question, imageWasChanged, questionChange, questionRepo);

        base.Create(questionChange);
    }

    public void Create(Question question, QuestionRepo questionRepo)
    {
        var questionChange = new QuestionChange
        {
            Question = question,
            Type = QuestionChangeType.Create,
            AuthorId = question.Creator == null ? -1 : question.Creator.Id,
            DataVersion = 1
        };

        SetData(question, true, questionChange,questionRepo);

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