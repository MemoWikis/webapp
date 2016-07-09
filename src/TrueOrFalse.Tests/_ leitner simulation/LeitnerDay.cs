using System.Collections.Generic;

public class LeitnerDay
{
    public int Number;
    public IEnumerable<LeitnerBox> BoxesBefore;
    public IEnumerable<LeitnerBox> BoxesAfter { get; set; }
}