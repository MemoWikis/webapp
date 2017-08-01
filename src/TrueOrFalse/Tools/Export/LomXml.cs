using System.Linq;
using System.Xml.Linq;
using TrueOrFalse.Frontend.Web.Code;

public class LomXml
{
    public static string From(Category category)
    {
        var langAttribute = new XAttribute("language", "de");
        var sourceElementLom = new XElement("source", "LOMv1.0");
        var sourceElementLre = new XElement("source", "LREv3.0");

        var xDoc = new XDocument(
            new XElement("lom",
                GetGeneral(category, langAttribute, sourceElementLom),
                GetLifecycle(langAttribute, sourceElementLom),
                GetMetaMetadata(),//???
                GetTechnical(category),
                GetEducational()
            )
        );

        return xDoc.ToString();
    }

    private static XElement GetTechnical(Category category)
    {
        return new XElement("technical",
            //Format, size
            new XElement("location", Links.CategoryDetail(category))
        );
    }

    private static XElement GetMetaMetadata()
    {
        return new XElement("metaMetadata","");
    }

    private static XElement GetEducational()
    {
        return new XElement("educational",
            new XElement("learningResourceType", LomEducationalEndUser.Learner.GetValue()),
            new XElement("context","")//???
        );
    }

    private static XElement GetLifecycle(XAttribute langAttribute, XElement sourceElementLom)
    {
        return new XElement("lifeCycle",
            new XElement("version",
                new XElement("string", "1.0", langAttribute)
            ),
            new XElement("status",
                sourceElementLom,
                new XElement("value", LomLifecycleStatus.Final.GetValue())
            ),
            new XElement("contribute",
                new XElement("role", 
                    sourceElementLom,
                    new XElement("value", LomLifecycleRole.Author.GetValue())
                )
                ,
                new XElement("entity", new XCData("test"))
            )
            //Liste von "contribute"
            //Unter "entity" vCard
            //Date
            //...
        );
    }

    private static XElement GetGeneral(Category category, XAttribute langAttribute, XElement sourceElementLom)
    {
        return new XElement("general", 
            new XElement("identifier",
                new XElement("catalog", "memucho"),
                new XElement("entry", category.Id)
            ),
            new XElement("title", 
                new XElement("string", category.Name.Truncate(1000), langAttribute)
            ),
            new XElement("language", "de"),
            new XElement("description", 
                new XElement("string", category.Description.Truncate(2000), langAttribute)
            ),
            category.ParentCategories().Select(c => 
                new XElement("keyword", 
                    new XElement("string", c.Name, langAttribute)
                )
            ).ToList(),
            new XElement("structure",
                sourceElementLom,
                new XElement("value", LomGeneralStructure.Atomic.GetValue())
            ),
            new XElement("aggregationLevel",
                sourceElementLom,
                new XElement("value", LomAggregationLevel.Level3Course.GetValue())
            )
        );
    }
}