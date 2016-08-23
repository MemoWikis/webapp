using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class LeitnerBox
{
    public int Number;
    public List<LeitnerQuestion> Questions = new List<LeitnerQuestion>();

    public int RepeatEvery;

    public LeitnerBox NextBox;
    public bool ToRepeat;

    public static IList<LeitnerBox> CreateBoxes()
    {
        var allBoxes = Enumerable
            .Range(start:1, count:5)
            .Select(i => new LeitnerBox{
                Number = i,
            })
            .ToList();

        for (var i = 0; i < allBoxes.Count; i++)
            if(i + 1 < allBoxes.Count)
                allBoxes[i].NextBox = allBoxes[i + 1];

        allBoxes[0].RepeatEvery = 1;
        allBoxes[1].RepeatEvery = 3;
        allBoxes[2].RepeatEvery = 9;
        allBoxes[3].RepeatEvery = 30;
        allBoxes[4].RepeatEvery = 90;

        return allBoxes;
    }

    public void Remove(LeitnerQuestion leitnerQuestion)
    {
        Questions.Remove(leitnerQuestion);
    }

    public void MoveToNextBox(LeitnerQuestion question)
    {
        Remove(question);
        NextBox.Questions.Add(question);
        question.Box = NextBox;
    }

    public void MoveToBox(LeitnerQuestion question, LeitnerBox box)
    {
        Remove(question);
        box.Questions.Add(question);
        question.Box = box;
    }
}