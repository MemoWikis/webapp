using System.Collections.Generic;
using System.Linq;

public class GenerateQuestionFeatures
{
    public static IList<QuestionFeature> Run()
    {
        var features = new List<QuestionFeature>();

        features.Add(new QuestionFeature
        {
            Id2 = "NoBrainer",
            Name = "No brainer",
            DoesApply = param =>
            {
                var historiesPerUser = 
                    param.AnswerHistory
                        .GroupBy(a => a.UserId)
                        .Select(g => g.ToList())
                        .ToList();

                var allFirstAnswers = historiesPerUser.Select(h => h.First()).ToList();
                var allSecondOrLaterAnswers = historiesPerUser.SelectMany(h => h.Skip(1)).ToList();

                if (allFirstAnswers.Count() <= 5 || allSecondOrLaterAnswers.Count() <= 5)
                    return false;

                return
                    allFirstAnswers.AverageCorrectness() >= 0.8 &&
                    allSecondOrLaterAnswers.AverageCorrectness() >= 0.9;
            }
        });

        features.Add(new QuestionFeature
        {
            Id2 = "EasyToLearn",
            Name = "Einfach zu lernen",
            DoesApply = param =>
            {
                var historiesPerUser =
                    param.AnswerHistory
                        .GroupBy(a => a.UserId)
                        .Select(g => g.ToList())
                        .ToList();

                return false;
            }
        });

        return features;
    }
}