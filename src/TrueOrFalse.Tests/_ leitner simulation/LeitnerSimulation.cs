using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Utils;
using NHibernate.Util;

public class LeitnerSimulation
{
    public IList<LeitnerDay> Days = new List<LeitnerDay>();
    public static int InitialProbability { get; private set; }

    public void Start(int numberOfDays = 10, int numberOfQuestions = 100, int initialProbability = 50)
    {
        var leitnerBoxes = LeitnerBox.CreateBoxes();
        var leitnerQuestions = LeitnerQuestion.CreateQuestions(numberOfQuestions);
        InitialProbability = initialProbability;

        MoveToFirstBox(leitnerBoxes, leitnerQuestions);

        for (var i = 0; i < numberOfDays; i++)
            AdvanceDay(leitnerBoxes);
    }

    private static void MoveToFirstBox(IList<LeitnerBox> leitnerBoxes, IList<LeitnerQuestion> leitnerQuestions)
    {
        var box1 = leitnerBoxes.ByNumber(1);
        leitnerQuestions.ForEach(q => q.Box = box1);
        box1.Questions.AddRange(leitnerQuestions);
    }

    public void AdvanceDay(IList<LeitnerBox> boxes)
    {
        var currentDay = Days.Count + 1;
        boxes.ForEach(b => b.ToRepeat = false);        
        var boxesToRepeat = boxes.Where(x => currentDay % x.RepeatEvery== 0).ToList();
        boxesToRepeat.ForEach(b => b.ToRepeat = true);

        var firstBox = boxes.ByNumber(1);
        var boxesBefore = boxes.DeepClone();

        var questionsToMoveToFirstBox = new List<LeitnerQuestion>();
        var questionToMoveToNextBox = new List<LeitnerQuestion>();

        foreach (var box in boxesToRepeat.OrderByDescending(b => b.Number))
        {
            foreach (var question in box.Questions)
            {
                if (question.Answer(currentDay))
                    questionToMoveToNextBox.Add(question);
                else
                    questionsToMoveToFirstBox.Add(question);
            }
        }

        questionToMoveToNextBox.ForEach(q => q.Box.MoveToNextBox(q));
        questionsToMoveToFirstBox.ForEach(q => q.Box.MoveToBox(q, firstBox));

        Days.Add(new LeitnerDay
        {
            Number = currentDay,
            BoxesBefore = boxesBefore,
            BoxesAfter = boxes.DeepClone()

        });

    }
}