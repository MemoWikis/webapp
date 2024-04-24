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
        ISession nhibernateSession)
    {
        _nhibernateSession = nhibernateSession;
        //TextHtml is missing here
        QuestionText = question.Text;
        QuestionTextExtended = question.TextExtended;
        ImageWasChanged = imageWasChanged;
        License = question.License;
        Visibility = question.Visibility;
        Solution = question.Solution;
        SolutionDescription = question.Description;
        SolutionMetadataJson = question.SolutionMetadataJson;
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
        question.TextExtended = this.QuestionTextExtended;
        question.License = this.License;
        question.Visibility = this.Visibility;
        question.Solution = this.Solution;
        question.Description = this.SolutionDescription;
        question.SolutionMetadataJson = this.SolutionMetadataJson;

        return question;
    }
}