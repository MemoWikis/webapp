using System.Collections.Generic;
using System.Linq;

public class LeitnerDay
{
    public int Number;
    public IEnumerable<LeitnerBox> BoxesBefore;
    public IEnumerable<LeitnerBox> BoxesAfter { get; set; }

    public int GetSumOfRepetitions() => 
        BoxesBefore.GetBoxesToRepeat(Number).Sum(x => x.Questions.Count);
}