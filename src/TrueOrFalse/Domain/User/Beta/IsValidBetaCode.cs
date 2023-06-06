public class IsValidBetaCode
{
    public static bool Yes(string betaCodeIn)
    {
        if (String.IsNullOrEmpty(betaCodeIn))
            return false;

        var betaCode = Settings.BetaCode().Replace(" ", "").ToLower();

        return betaCodeIn.Replace(" ", "").ToLower() == betaCode;
    }
}