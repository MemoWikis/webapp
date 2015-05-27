using System.Collections.Specialized;
using System.ComponentModel;

public class QuestionSolutionExact : QuestionSolution
{
    [DisplayName("Antwort")]
    public string Text { get; set; }

    public string MetadataSolutionJson { get; set; }

    public override bool IsCorrect(string answer)
    {
        var solutionMetadata = new SolutionMetadata{Json = MetadataSolutionJson};
        if (solutionMetadata.IsDate)
        {
            var metaDate = solutionMetadata.GetAsDate();
            var dateFromInput = DateAnswerParser.Run(answer);
            var dateAnswer = DateAnswerParser.Run(Text);

            if (!dateFromInput.IsValid)
                return false;

            if ((int)dateFromInput.Precision > (int)metaDate.Precision)
                return false;

            return dateAnswer.Valid(dateFromInput, metaDate.Precision);
        }
        return Text == answer;
    }

    public override string CorrectAnswer()
    {
        return Text;
    }

    public void FillFromPostData(NameValueCollection postData)
    {
        Text = postData["Text"];
        MetadataSolutionJson = postData["MetadataSolutionJson"];
    }
}