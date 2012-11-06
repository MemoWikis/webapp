using System;
using System.Web.Script.Serialization;
using TrueOrFalse;

public class GetQuestionSolution
{
    public QuestionSolution Run(QuestionSolutionType type, string solution)
    {
        var serializer = new JavaScriptSerializer();
        switch (type)
        {
            case QuestionSolutionType.Text:
                return new QuestionSoulutionExact {Text = solution};

            case QuestionSolutionType.Sequence:
                return serializer.Deserialize<QuestionSolutionSequence>(solution);

            case QuestionSolutionType.MultipleChoice:
                return serializer.Deserialize<QuestionSolutionMultipleChoice>(solution);
        }

        throw new NotImplementedException(string.Format("Solution Type not implemented: {0}", type));
    }
}