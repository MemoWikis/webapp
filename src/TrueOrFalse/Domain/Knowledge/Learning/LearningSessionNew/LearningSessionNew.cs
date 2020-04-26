using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LearningSessionNew
{
    public static List<Question> GetRandom(int amountQuestions, List<Question> questions)
    {
        if (amountQuestions > questions.Count || amountQuestions < 0)
            return new List<Question>();

        var result = new List<Question>();
        var rand = new Random();
        for (var i = 0; i < amountQuestions; i++)
        {
            var nextQuestion = rand.Next(0, questions.Count - 1);
            questions.RemoveAt(nextQuestion);
            result.Add(questions[nextQuestion]);
        }

        return result;
    }
}