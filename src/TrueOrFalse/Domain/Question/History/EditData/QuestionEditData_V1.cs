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
}