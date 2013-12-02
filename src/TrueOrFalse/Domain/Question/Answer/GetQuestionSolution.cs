using System;
using System.Web.Script.Serialization;
using TrueOrFalse;

public class GetQuestionSolution
{
    public QuestionSolution Run(SolutionType type, string solution)
    {
        var serializer = new JavaScriptSerializer();
        switch (type)
        {
            case SolutionType.Text:
                return new QuestionSolutionExact {Text = solution};

            case SolutionType.Sequence:
                return serializer.Deserialize<QuestionSolutionSequence>(solution);

            case SolutionType.MultipleChoice:
                return serializer.Deserialize<QuestionSolutionMultipleChoice>(solution);
        }

        throw new NotImplementedException(string.Format("Solution Type not implemented: {0}", type));
    }
}