using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class KnowledgeSummary
{
    public int Secure = 0;
    public int Weak  = 0;
    public int Unknown = 0;

    public int Total{ get { return Secure + Weak + Unknown; }}
}

