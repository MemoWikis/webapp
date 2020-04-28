using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LearningSessionNew
{
    private static Random rng = new Random();
    public static List<Question> GetRandom(int amountQuestions, List<Question> questions, bool getAllQuestions)
    {
        if (amountQuestions > questions.Count || amountQuestions < 0)
            return new List<Question>();

        if (!getAllQuestions)
        {

        }

        questions.Shuffle();
        //TODO: DeleteQuestions /
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
    public static void Shuffle(IList<Question> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Question value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}