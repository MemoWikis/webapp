public static class NullableExtensions
{
    public static bool IsTrue(this bool? val)
    {
        return val == true;
    }
    public static bool IsNull(this bool? val)
    {
        return val == null;
    }
}