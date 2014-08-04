using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

public class KnowledgeSummary
{
    public int Secure = 0;
    public int SecurePercentage { get { return (int) Math.Round(Secure/(decimal) Total * 100); } }
    
    public int Weak  = 0;
    public int WeakPercentage { get { return (int)Math.Round(Weak/(decimal)Total * 100); } }

    public int Unknown = 0;
    public int UnknownPercentage { get { return (int)Math.Round(Unknown/(decimal)Total * 100); } }

    public int Total{ get { return Secure + Weak + Unknown; }}
}

