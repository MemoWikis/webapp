using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class KnowledgeSummary
{
    public int Secure;
    public int Weak;
    public int Unknown;

    public int Total{ get { return Secure + Weak + Unknown; }}
}

