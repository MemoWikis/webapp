using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class ObjectExtensions
{
    public static T DeepClone<T>(this T input) where T : class
    {
        if (input == null)
            return default;

        var binaryFormatter = new BinaryFormatter();
        using (MemoryStream memoryStream = new MemoryStream())
        {
            binaryFormatter.Serialize(memoryStream, input);
            memoryStream.Seek(0L, SeekOrigin.Begin);
            return (T)binaryFormatter.Deserialize(memoryStream);
        }
    }
}

