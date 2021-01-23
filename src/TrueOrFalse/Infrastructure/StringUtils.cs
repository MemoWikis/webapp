public static class StringUtils
{
    public static string PluralSuffix(int numberItems, string pluralSuffix, string singularSuffix = "", string divergentZeroSuffix = null)
    {
        if (numberItems == 1)
            return singularSuffix;

        if (numberItems == 0)
            return divergentZeroSuffix ?? pluralSuffix;

        return pluralSuffix;
    }
}

