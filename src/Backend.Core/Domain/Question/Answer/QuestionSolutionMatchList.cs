using System.Collections.Specialized;
using Newtonsoft.Json;

public class QuestionSolutionMatchList : QuestionSolution
{
    private const string PairSeperator = "%pairseperator%";
    private const string ElementSeperator = "%elementseperator%";
    public List<Pair> Pairs = new List<Pair>();
    public List<ElementRight> RightElements = new List<ElementRight>();
    public bool IsSolutionOrdered;

    public void FillFromPostData(NameValueCollection postData)
    {
        List<string> leftElementText =
            (
                from key in postData.AllKeys
                where key.StartsWith("LeftElement-")
                select postData.Get(key.Trim())
            )
            .ToList();

        List<string> rightPairElementText =
            (
                from key in postData.AllKeys
                where key.StartsWith("RightPairElement-")
                select postData.Get(key.Trim())
                    .Replace(" ", " ") //Replaces non-breaking-spaces with normal spaces
            )
            .ToList();

        List<string> rightElementText =
            (
                from key in postData.AllKeys
                where key.StartsWith("RightElement-")
                select postData.Get(key.Trim())
            )
            .ToList();

        for (int i = 0; i < leftElementText.Count; i++)
        {
            Pairs.Add(new Pair
            {
                ElementLeft = new ElementLeft { Text = leftElementText[i] },
                ElementRight = new ElementRight { Text = rightPairElementText[i] }
            });
        }

        foreach (var singleRightElementText in rightElementText)
        {
            RightElements.Add(new ElementRight { Text = singleRightElementText });
        }

        IsSolutionOrdered = postData["isSolutionRandomlyOrdered"] != "";

        TrimElementTexts();
    }

    public static MatchListAnswerPairs? DeserializeMatchListAnswer(string answerJson)
    {
        return JsonConvert.DeserializeObject<MatchListAnswerPairs>(answerJson);
    }

    public override bool IsCorrect(string answer)
    {
        if (answer == "")
            return false;
        var answerObject = DeserializeMatchListAnswer(answer);
        var answerPairs = answerObject.Pairs.OrderBy(t => t.ElementLeft.Text).ToList();

        TrimElementTexts();
        var questionPairs = Pairs.OrderBy(t => t.ElementLeft.Text).ToList();
        for (int i = 0; i < questionPairs.Count; i++)
        {
            if (questionPairs[i].ElementRight.Text == "Keine Zuordnung")
            {
                questionPairs.RemoveAt(i);
                i--;
            }
        }

        bool answerCorrect = true;
        if (questionPairs.Count != answerPairs.Count)
            answerCorrect = false;
        else
        {
            for (int i = 0; i < questionPairs.Count; i++)
            {
                bool isSameElementLeft =
                    questionPairs[i].ElementLeft.Text == answerPairs[i].ElementLeft.Text;
                bool isSameElementRight = questionPairs[i].ElementRight.Text ==
                                          answerPairs[i].ElementRight.Text;
                if (isSameElementLeft && isSameElementRight)
                    continue;
                answerCorrect = false;
            }
        }

        return answerCorrect;
    }

    public override string CorrectAnswer()
    {
        TrimElementTexts();
        string correctAnswerMessage = PairSeperator;
        foreach (var pair in Pairs)
            correctAnswerMessage += pair.ElementLeft.Text + ElementSeperator +
                                    pair.ElementRight.Text + PairSeperator;

        return correctAnswerMessage;
    }

    public override string GetCorrectAnswerAsHtml()
    {
        var htmlListItems = CorrectAnswer()
            .Split(new[] { PairSeperator }, StringSplitOptions.RemoveEmptyEntries)
            .Select(a =>
                $"<li>{String.Join(" - ", a.Split(new[] { ElementSeperator }, StringSplitOptions.RemoveEmptyEntries))}</li>")
            .Aggregate((a, b) => a + b);

        return $"<ul>{htmlListItems}</ul>";
    }

    public void TrimElementTexts()
    {
        foreach (var pair in Pairs)
        {
            pair.ElementLeft.Text = pair.ElementLeft.Text.Trim();
            pair.ElementRight.Text = pair.ElementRight.Text.Trim();
        }

        foreach (var rightElement in RightElements)
        {
            rightElement.Text = rightElement.Text.Trim();
        }
    }
}

public class MatchListAnswerPairs
{
    public List<Pair> Pairs { get; set; }
}