using System.Web.Script.Serialization;
using TrueOrFalse.Core;

public class GetQuestionSolutionModel
{
    public static object Run(QuestionSolutionType type, string solution)
    {
        object model = null;
        var serializer = new JavaScriptSerializer();

        switch (type)
        {
            case QuestionSolutionType.Sequence:
                model = serializer.Deserialize<AnswerTypeSequenceModel>(solution);
                break;

            case QuestionSolutionType.MultipleChoice:
                model = serializer.Deserialize<AnswerTypeMulitpleChoiceModel>(solution);
                break;
        }
        return model;
    }
}