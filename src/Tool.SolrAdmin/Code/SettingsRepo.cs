using System;
using System.IO;
using Newtonsoft.Json;

public class SettingsRepo
{
    public static string _filePath
    {
        get
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");
            if (!File.Exists(filePath)){
                using (File.Create(filePath)) { }
            }

            return filePath;
        }
    }

    public static Settings Load()
    {
        var result = 
            JsonConvert.DeserializeObject<Settings>(File.ReadAllText(_filePath)) ?? 
            new Settings();

        return result;
    }

    public static void Save(Settings settings)
    {
        File.WriteAllText(_filePath, JsonConvert.SerializeObject(settings));
    }
}