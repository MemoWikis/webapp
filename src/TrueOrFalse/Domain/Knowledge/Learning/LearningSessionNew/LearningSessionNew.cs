using System.Collections.Generic;
using System.Linq;

public class LearningSessionNew
{
    public static List<Question> GetRandom(int amountQuestions, List<Question> questions)
    {
        questions.Shuffle();
        if (amountQuestions > questions.Count)
        {
            Logg.r().Error("LearningSession amountQuestion is too high");
            return questions;
        }
        if (amountQuestions <= 0)
        {
            Logg.r().Error("LearningSession amountQuestion is too low");
            return questions.Take(1).ToList();
        }

        questions.Shuffle();
        var amountQuestionsToDelete = questions.Count - amountQuestions;
        questions.RemoveRange(0, amountQuestionsToDelete);
        return questions;

    }

    public static List<Question> GetDifficultQuestions(List<Question> questions)
    {
        return questions.Where(q => q.CorrectnessProbability >= 50).ToList();
    }

    public static List<Question> GetSimpleQuestions(List<Question> questions)
    {
        return questions.Where(q => q.CorrectnessProbability <= 50).ToList();
    }
}