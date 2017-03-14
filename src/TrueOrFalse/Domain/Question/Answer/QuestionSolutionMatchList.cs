using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TrueOrFalse.Domain.Question.SolutionType.MatchList;

public class QuestionSolutionMatchList : QuestionSolution
{
    public List<Pair> Pairs = new List<Pair>();
    public List<ElementRight> RightElements = new List<ElementRight>();

    public void FillFromPostData(NameValueCollection postData)
    {
        List<string> LeftElementText =
        (
            from key in postData.AllKeys
            where key.StartsWith("LeftElement-")
            select postData.Get(key)
        )
        .ToList();

        List<string> RightPairElementText =
        (
            from key in postData.AllKeys
            where key.StartsWith("RightPairElement-")
            select postData.Get(key)
        )
        .ToList();

        List<string> RightElementText =
        (
            from key in postData.AllKeys
            where key.StartsWith("RightElement-")
            select postData.Get(key)
        )
        .ToList();

        for (int i = 0; i < LeftElementText.Count; i++)
        {
            Pairs.Add(new Pair()
            {
                ElementLeft = new ElementLeft() {Text = LeftElementText[i]},
                ElementRight = new ElementRight() {Text = RightPairElementText[i]}
            });
        }

        foreach (var singleRightElementText in RightElementText)
        {
            RightElements.Add(new ElementRight() {Text = singleRightElementText});
        }
    }

    public override bool IsCorrect(string answer)
    {
        //string[] Answers = answer.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
        //string[] Solutions = this.CorrectAnswer().Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
        //return Enumerable.SequenceEqual(Answers.OrderBy(t => t), Solutions.OrderBy(t => t));
        return false;
    }

    public override string CorrectAnswer()
    {
        //string CorrectAnswer = "";
        //foreach (var SingleChoice in this.Choices)
        //{
        //    if (SingleChoice.IsCorrect == true)
        //    {
        //        CorrectAnswer += SingleChoice.Text;
        //        if (SingleChoice != this.Choices[(this.Choices.Count - 1)])
        //            CorrectAnswer += ", ";
        //    }
        //}
        //return CorrectAnswer;
        return "false";
    }
}