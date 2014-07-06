using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SetActiveMemory
{
    public int TotalQuestions;
    public int TotalInActiveMemory;

    public int TotalNotInMemory{
        get { return TotalQuestions - TotalInActiveMemory; }
    }
}

