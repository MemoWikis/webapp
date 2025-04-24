using System.Collections.Generic;
using System.Linq;

public static class LeitnerBoxListExts
{
    public static LeitnerBox ByNumber(this IEnumerable<LeitnerBox> boxes, int boxNumber) 
        => boxes.First(box => box.Number == boxNumber);

    public static IList<LeitnerBox> GetBoxesToRepeat(this IEnumerable<LeitnerBox> boxes, int currentDay) => 
        boxes.Where(x => currentDay % x.RepeatEvery == 0).ToList();
}