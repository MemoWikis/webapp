using System.Collections.Generic;

public class InfoBoxTemplate
{
    public string TemplateName;
    public string DescriptionParamaterName = "Description";
    public string AuthorParameterName = "Author";

    /// <summary>
    /// Should use the attribution from licence Header. See sample below:
    /// 
    /// == {{int:license-header}} ==
    /// {{Cc-by-sa-3.0-de|attribution=Bundesarchiv, B 145 Bild-F078072-0004 / Katherine Young / CC-BY-SA 3.0}}
    ///
    /// The Attribution should be: "Bundesarchiv, B 145 Bild-F078072-0004 / Katherine Young / CC-BY-SA 3.0"
    /// </summary>
    public bool UseAttributionFromLicense;

    public static List<InfoBoxTemplate> GetAllInfoBoxTemplates()
    {
        return new List<InfoBoxTemplate>
        {
            new InfoBoxTemplate
            {
                TemplateName = "Information",
            },

            new InfoBoxTemplate
            {
                TemplateName = "Infobox aircraft image",
                AuthorParameterName = "imageauthor"
            },
            //Bundesarchiv key: BArch-image (result.InfoTemplate.ParamByKey("BArch-image"))

            new InfoBoxTemplate
            {
                TemplateName = "BArch-image",
                AuthorParameterName = "imageauthor"
            },
        };
    } 
}