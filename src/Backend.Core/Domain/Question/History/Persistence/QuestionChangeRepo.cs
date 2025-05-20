using NHibernate;

public class QuestionChangeRepo : RepositoryDbBase<QuestionChange>
{
    private readonly SessionUser _sessionUser;
    private readonly QuestionReadingRepo _questionReadingRepo;

    public QuestionChangeRepo(ISession session, SessionUser sessionUser, QuestionReadingRepo questionReadingRepo) : base(session)
    {
        _sessionUser = sessionUser;
        _questionReadingRepo = questionReadingRepo;
    }

    public int AddDeleteEntry(Question question, int userId)
    {
        var questionChange = new QuestionChange
        {
            Question = question,
            AuthorId = userId,
            Type = QuestionChangeType.Delete,
            DataVersion = 1
        };

        base.Create(questionChange);
        return questionChange.Id;
    }

    public virtual void SetData(Question question, bool imageWasChanged, QuestionChange questionChange, int[]? commentIds = null)
    {
        switch (questionChange.DataVersion)
        {
            case 1:
                questionChange.Data = new QuestionEditData_V1(question, imageWasChanged, _session, commentIds).ToJson();
                break;

            default:
                throw new ArgumentOutOfRangeException($"Invalid data version number {questionChange.DataVersion} for question change id {questionChange.Id}");
        }
    }

    public void AddCreateEntry(Question question) => AddUpdateOrCreateEntry(question, QuestionChangeType.Create, question.Creator, imageWasChanged: true);
    public void AddUpdateEntry(Question question, User author = null, bool imageWasChanged = false) => AddUpdateOrCreateEntry(question, QuestionChangeType.Update, author, imageWasChanged);

    private void AddUpdateOrCreateEntry(Question question,
        QuestionChangeType questionChangeType,
        User author,
        bool imageWasChanged,
        int[]? commentIds = null)
    {
        var questionChange = new QuestionChange
        {
            Question = question,
            Type = questionChangeType,
            AuthorId = author != null ? author.Id : _sessionUser.IsLoggedIn ? _sessionUser.UserId : default,
            DataVersion = 1
        };

        SetData(question, imageWasChanged, questionChange, commentIds);

        base.Create(questionChange);
        var questionCacheItem = EntityCache.GetQuestion(question.Id);
        if (questionCacheItem != null)
            questionCacheItem.AddQuestionChangeToPageChangeCacheItems(questionChange);
    }

    public void AddCommentEntry(int questionId, int authorId, int[] commentIds) =>
        AddUpdateEntryForComment(questionId, QuestionChangeType.AddComment, authorId, commentIds);

    private void AddUpdateEntryForComment(int questionId,
        QuestionChangeType questionChangeType,
        int authorId,
        int[]? commentIds = null)
    {
        var question = _questionReadingRepo.GetById(questionId);
        if (question != null)
        {
            var questionChange = new QuestionChange
            {
                Question = question,
                Type = questionChangeType,
                AuthorId = authorId,
                DataVersion = 1,
            };

            SetData(question, imageWasChanged: false, questionChange, commentIds);

            base.Create(questionChange);
            var questionCacheItem = EntityCache.GetQuestion(question.Id);
            if (questionCacheItem != null)
                questionCacheItem.AddQuestionChangeToPageChangeCacheItems(questionChange);
        }
    }

    public QuestionChange GetByIdEager(int questionChangeId)
    {
        return _session
            .QueryOver<QuestionChange>()
            .Where(cc => cc.Id == questionChangeId)
            .Left.JoinQueryOver(q => q.Question)
            .SingleOrDefault();
    }

    public QuestionChange? GetByQuestionId(int questionId) =>
        _session.QueryOver<QuestionChange>().Where(cc => cc.Question.Id == questionId).SingleOrDefault();
}