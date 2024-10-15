using Newtonsoft.Json;
using NHibernate;

public class QuestionEditData_V1 : QuestionEditData
{
    private readonly ISession _nhibernateSession;

    public QuestionEditData_V1()
    {
    }

    public QuestionEditData_V1(
        Question question,
        bool imageWasChanged,
        ISession nhibernateSession,
        int[]? commentIds = null)
    {
        _nhibernateSession = nhibernateSession;
        QuestionText = question.Text;
        TextHtml = question.TextHtml;
        QuestionTextExtended = question.TextExtended;
        TextExtendedHtml = question.TextExtendedHtml;
        Description = question.Description;
        DescriptionHtml = question.DescriptionHtml;
        ImageWasChanged = imageWasChanged;
        License = question.License;
        Visibility = question.Visibility;
        Solution = question.Solution;
        SolutionDescription = question.Description;
        SolutionMetadataJson = question.SolutionMetadataJson;
        CommentIds = commentIds;
    }

    public override string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static QuestionEditData_V1 CreateFromJson(string json)
    {
        return JsonConvert.DeserializeObject<QuestionEditData_V1>(json);
    }

    public override Question ToQuestion(Question question)
    {
        // Query Categories and References properties to load and thus prevent an
        // NHibernate.LazyInitializationException
        question.Categories.ToList();
        question.References.ToList();

        _nhibernateSession.Evict(question);

        question.Text = this.QuestionText;
        question.TextHtml = this.TextHtml;
        question.TextExtended = this.QuestionTextExtended;
        question.TextExtendedHtml = this.TextExtendedHtml;
        question.Description = this.Description;
        question.DescriptionHtml = this.DescriptionHtml;
        question.License = this.License;
        question.Visibility = this.Visibility;
        question.Solution = this.Solution;
        question.Description = this.SolutionDescription;
        question.SolutionMetadataJson = this.SolutionMetadataJson;

        return question;
    }

    public override QuestionCacheItem ToQuestionCacheItem(int questionId)
    {
        var question = EntityCache.GetQuestion(questionId);
        question = question == null ? new QuestionCacheItem() : question;

        question.Text = QuestionText;
        question.TextHtml = TextHtml;
        question.TextExtended = QuestionTextExtended;
        question.TextExtendedHtml = TextExtendedHtml;
        question.Description = Description;
        question.DescriptionHtml = DescriptionHtml;
        question.License = License;
        question.Visibility = Visibility;
        question.Solution = Solution;
        question.Description = SolutionDescription;
        question.SolutionMetadataJson = SolutionMetadataJson;

        return question;
    }
}