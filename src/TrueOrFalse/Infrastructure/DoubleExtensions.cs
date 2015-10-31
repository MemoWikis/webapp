public static class DoubleExtensions
{
    public static bool IsBetween(this double input, double isAbove, double isBelowOrEqual)
    {
        return input >= isAbove && input < isBelowOrEqual;
    }
}