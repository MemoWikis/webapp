using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Utils;

public class LeitnerSimulation
{
    public IList<LeitnerDay> Days = new List<LeitnerDay>();

    public void Start(int numberOfDays = 10)
    {
        var leitnerBoxes = LeitnerBox.CreateBoxes();
        var leitnerQuestions = LeitnerQuestion.CreateQuestions(100);

        leitnerBoxes.AddToBox(1, leitnerQuestions);

        for (var i = 0; i < numberOfDays; i++)
            AdvanceDay(leitnerBoxes);
    }

    public void AdvanceDay(IList<LeitnerBox> boxes)
    {
        var currentDay = Days.Count + 1;

        Days.Add(new LeitnerDay
        {
            Number = Days.Count + 1,
            Boxes = boxes.DeepClone()
        });

        var boxesToRepeat = boxes.Where(x => currentDay % x.RepeatEvery== 0);
        var firstBox = boxes.ByNumber(1);

        foreach (var box in boxesToRepeat.OrderByDescending(b => b.Number))
            for (var i = 0; i < box.Questions.Count; i++)
            {
                var question = box.Questions[i];

                if (question.Answer(currentDay))
                    box.MoveToNextBox(question);
                else
                    box.MoveToBox(question, firstBox);
            }

    }
}