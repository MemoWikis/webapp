using System.Xml.Linq;

public class LomXml
{
    public static string From(Category category)
    {
        var xDoc = new XDocument(
            new XElement("general", 
                new XElement("identifier",
                    new XElement("catalog", "memucho"),
                    new XElement("entry", category.Id)
                ),
                new XElement("title", 
                    new XElement("string", category.Name, 
                        new XAttribute("language", "de")))
            )
        );

        return xDoc.ToString();
    }
}