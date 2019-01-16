using System.Linq;
using Newtonsoft.Json;

public class QuestionEditData_V1 : QuestionEditData
{
    public QuestionEditData_V1(){}

    public QuestionEditData_V1(Question question)
    {
        QuestionText = question.Text;
        QuestionTextExtended = question.TextExtended;
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

    public override Question ToQuestion(int questionId)
    {
        var question = Sl.QuestionRepo.GetById(questionId);

        // Query Categories and References properties to load and thus prevent an
        // NHibernate.LazyInitializationException
        question.Categories.ToList();
        question.References.ToList();

        Sl.Session.Evict(question);

        //TODO FK ToQuestion
        //question.IsHistoric = true;
        question.Text = this.QuestionText;
        question.TextExtended = this.QuestionTextExtended;
        question.License = this.License;
        question.Visibility = this.Visibility;
        question.Solution = this.Solution;
        question.Description = this.SolutionDescription;
        question.SolutionMetadataJson = this.SolutionMetadataJson;
        
        // Historic question relations cannot be loaded because we do not have archive data and
        // loading them leads to nasty conflicts and nuisance with NHibernate.

        return question;
    }
}