public static class StringUtils
{

    public static string Plural(int amount, string pluralSuffix, string singularSuffix = "", string zeroSuffix = "")
    {
        if (amount > 1)
            return pluralSuffix;
        return amount == 0 ? zeroSuffix : singularSuffix;
    }

}

