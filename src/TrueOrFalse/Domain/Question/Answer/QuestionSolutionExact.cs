using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

public class QuestionSolutionExact : QuestionSolution
{
    [DisplayName("Antwort")]
    public string Text { get; set; }

    public string MetadataSolutionJson { get; set; }

    public override bool IsCorrect(string answer)
    {
        answer = answer.Trim();
        var solutionMetadata = new SolutionMetadata{Json = MetadataSolutionJson};
        if (solutionMetadata.IsDate)
        {
            var metaDate = solutionMetadata.GetForDate();
            var dateFromInput = DateAnswerParser.Run(answer);
            var dateAnswer = DateAnswerParser.Run(Text);

            if (!dateFromInput.IsValid)
                return false;

            if ((int)dateFromInput.Precision > (int)metaDate.Precision)
                return false;

            return dateAnswer.Valid(dateFromInput, metaDate.Precision);
        }

        if (solutionMetadata.IsText)
        {
            var metaText = solutionMetadata.GetForText();

            if (!metaText.IsCaseSensitive && !metaText.IsExactInput)
            {
                answer = answer.ToLower();

                var answerWords = answer.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var solutionWords = Text.ToLower().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var removableWords = new[] { "der", "die", "das", "ein", "einer", "eine" };

                if(answerWords.All(word => removableWords.Any(removable => removable == word)))
                    return Text.ToLower() == answer;

                Func<IEnumerable<string>, string> fnWithoutRemovableWords = x => x
                    .Where(word => removableWords.All(removable => removable != word))
                    .Aggregate((a, b) => a + " " + b);

                return fnWithoutRemovableWords(solutionWords) == fnWithoutRemovableWords(answerWords);
            }
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

    public void FillFromPostData(string text, string metadataSolutionJson)
    {
        Text = text;
        MetadataSolutionJson = metadataSolutionJson;
    }
}