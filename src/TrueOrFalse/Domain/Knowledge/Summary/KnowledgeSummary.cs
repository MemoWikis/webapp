using System;

public class KnowledgeSummary
{
    public int Secure = 0;
    public int SecurePercentage { get { return Percentage(Secure); } }
    
    public int Weak  = 0;
    public int WeakPercentage { get { return Percentage(Weak); } }

    public int Unknown = 0;
    public int UnknownPercentage { get { return Percentage(Unknown); } }

    public int Total{ get { return Secure + Weak + Unknown; }}

    private int Percentage(int amount)
    {
        if (Total == 0 || amount == 0)
            return 0;

        return (int)Math.Round(amount / (decimal)Total * 100);
    }
}