public class NotNull
{
    public static T Run<T>(T type) where T : class, new() => 
        type ?? new T();
}