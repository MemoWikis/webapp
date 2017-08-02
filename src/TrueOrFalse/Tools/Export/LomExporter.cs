using System;
using System.IO;

public class LomExporter
{
    public static void AllToFileSystem()
    {
        lock ("c6fc2ccc-bf87-4b6f-b286-d2438e117cc1")
        {
            AllToFileSystem_();
        }    
    }

    private static void AllToFileSystem_()
    {
        var exportPath = Settings.LomExportPath;

        if (!Directory.Exists(exportPath))
            throw new Exception("directory does not exist");

        ExportCategories(exportPath);
    }

    private static void ExportCategories(string exportPath)
    {
        var listOfSelectedCategories = new[] {84, 724, 725, 841, 264, 265, 285, 606, 615, 618, 619, 607, 635, 644};
        var categories = Sl.CategoryRepo.GetByIds(listOfSelectedCategories);

        foreach (var category in categories)
        {
            var exportFilePath = Path.Combine(exportPath, $"topic-{category.Id}.xml");

            if (File.Exists(exportFilePath))
                File.Delete(exportFilePath);

            File.WriteAllText(exportFilePath, LomXml.From(category));
        }
    }
}

