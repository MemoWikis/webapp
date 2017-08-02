using System;
using System.Linq;
using System.Xml.Linq;
using TrueOrFalse.Frontend.Web.Code;

public class LomXml
{
    private static XAttribute _langAttributeDe => new XAttribute("language", "de");
    private static XElement _sourceElementLom => new XElement("source", "LOMv1.0");
    private static XElement _sourceElementLre => new XElement("source", "LREv3.0");
    private static readonly XCData _vCardCData = new XCData("BEGIN:vCard VERSION:3.0 FN:memucho.de N:memucho.de END:vcard");

    public static string From(Category category)
    {
        var xDoc = new XDocument(
            new XElement("lom",
                GetGeneral(category),
                GetLifecycle(category),
                GetMetaMetadata(category),
                GetTechnical(category),
                GetEducational(),
                GetRights()
            )
        );

        return xDoc.ToString();
    }

    private static XElement GetGeneral(Category category)
    {
        return new XElement("general",
            new XElement("identifier",
                new XElement("catalog", "memucho"),
                new XElement("entry", "thema-" + category.Id)
            ),
            new XElement("title",
                new XElement("string", category.Name.Truncate(1000), _langAttributeDe)
            ),
            new XElement("language", "de"),
            new XElement("description",
                new XElement("string", category.Description.Truncate(2000), _langAttributeDe)
            ),
            category.ParentCategories().Select(c =>
                new XElement("keyword",
                    new XElement("string", c.Name, _langAttributeDe)
                )
            ).ToList(),
            new XElement("structure",
                _sourceElementLom,
                new XElement("value", LomGeneralStructure.Atomic.GetValue())
            ),
            new XElement("aggregationLevel",
                _sourceElementLom,
                new XElement("value", LomAggregationLevel.Level3Course.GetValue())
            )
        );
    }

    private static XElement GetLifecycle(Category category)
    {
        return new XElement("lifeCycle",
            new XElement("version",
                new XElement("string", "1.0", _langAttributeDe)
            ),
            new XElement("status",
                _sourceElementLom,
                new XElement("value", LomLifecycleStatus.Final.GetValue())
            ),
            new XElement("contribute",
                new XElement("role",
                    _sourceElementLom,
                    new XElement("value", LomLifecycleRole.Author.GetValue())
                ),
                new XElement("entity", _vCardCData),
                new XElement("date",
                    new XElement("dateTime", category.DateCreated)
                )
            )
        );
    }

    private static XElement GetMetaMetadata(Category category)
    {
        return new XElement("metaMetadata",
            new XElement("identifier",
                new XElement("catalog", "memucho"),
                new XElement("entry", "metadata.memucho-thema-" + category.Id)
            ),
            new XElement("contribute", 
                new XElement("role",
                    _sourceElementLre,
                    new XElement("value", LomMetaMetadataRoleLre.Creator.GetValue())
                ),
                new XElement("entity", _vCardCData),
                new XElement("date", DateTime.Now)
            )
        );
    }

    private static XElement GetTechnical(Category category)
    {
        return new XElement("technical",
            new XElement("format", "text/html"),
            new XElement("location", "https://memucho.de" + Links.CategoryDetail(category))
        );
    }

    private static XElement GetEducational()
    {
        return new XElement("educational",
            new XElement("learningResourceType", "web page"),
            new XElement("context",
                _sourceElementLre,
                new XElement("value", LomEducationalContext.CompulsoryEducation)),
            new XElement("typicalAgeRange", "0-99")
        );
    }

    private static XElement GetRights()
    {
        return new XElement("rights",
            new XElement("copyrightAndOtherRestrictions",
                _sourceElementLom,
                new XElement("value", "yes")
            ),
            new XElement("description",
                new XElement("string", "CC BY, Autor: memucho", _langAttributeDe))
        );
    }

    //private static XElement GetClassification()
    //{
    //    return new XElement();
    //}
}
}