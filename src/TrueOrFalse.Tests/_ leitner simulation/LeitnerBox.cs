using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class LeitnerBox
{
    public int Number;
    public List<LeitnerQuestion> Questions = new List<LeitnerQuestion>();

    public int RepeatEvery;

    public static IList<LeitnerBox> CreateBoxes()
    {
        var result = Enumerable
            .Range(start:1, count:5)
            .Select(i => new LeitnerBox {Number = i})
            .ToList();

        result[0].RepeatEvery = 1;
        result[1].RepeatEvery = 3;
        result[2].RepeatEvery = 10;
        result[3].RepeatEvery = 30;
        result[4].RepeatEvery = 90;

        return result;
    }
}

public static class LeitnerBoxListExts
{
    public static void AddToBox(this IEnumerable<LeitnerBox> boxes, int boxNumber, IEnumerable<LeitnerQuestion> questions) 
        => boxes.ByNumber(boxNumber).Questions.AddRange(questions);

    public static LeitnerBox ByNumber(this IEnumerable<LeitnerBox> boxes, int boxNumber) 
        => boxes.First(box => box.Number == boxNumber);
}