public static class NullableExtensions
{
    public static bool IsTrue(this bool? val)
    {
        return val == true;
    }
    public static bool IsFalse(this bool? val)
    {
        return val == false;
    }
    public static bool IsNull(this bool? val)
    {
        return val == null;
    }
}