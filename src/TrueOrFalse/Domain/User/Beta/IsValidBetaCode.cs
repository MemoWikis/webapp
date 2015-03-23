using System;
using TrueOrFalse.Infrastructure;

public class IsValidBetaCode
{
    public static bool Yes(string betaCodeIn)
    {
        if (String.IsNullOrEmpty(betaCodeIn))
            return false;

        var betaCode = OverwrittenConfig.BetaCode().Replace(" ", "").ToLower();

        return betaCodeIn.Replace(" ", "").ToLower() == betaCode;
    }
}