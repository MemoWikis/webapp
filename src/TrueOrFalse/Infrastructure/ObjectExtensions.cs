using System.Text.Json;

public static class ObjectExtensions
{
    public static T DeepClone<T>(this T input) where T : class
    {
        if (input == null)
            return default;

        var json = JsonSerializer.Serialize(input);
        return JsonSerializer.Deserialize<T>(json);
    }
}