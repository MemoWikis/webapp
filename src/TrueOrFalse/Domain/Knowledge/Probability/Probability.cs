using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seedworks.Lib.Persistence;
using TrueOrFalse;

public class Probability : Entity
{
    public virtual decimal Percentage { get; set; }

    public virtual Question Question { get; set; }
    public virtual User User { get; set; }

    public virtual DateTime DateTimeCalculated { get; set; }

    public virtual int GetIntegerPercentage()
    {
        return (int)Math.Round(Percentage * 100, 0, MidpointRounding.AwayFromZero);
    }
}