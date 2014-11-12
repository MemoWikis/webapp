using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class InfoBoxTemplate
{
    public string TemplateName;
    public string DescriptionParamaterName = "Description";
    public string AuthorParameterName = "Author";

    public static List<InfoBoxTemplate> GetAllInfoBoxTemplates()
    {
        return new List<InfoBoxTemplate>()
        {
            new InfoBoxTemplate()
            {
                TemplateName = "Information",
            },

            new InfoBoxTemplate()
            {
                TemplateName = "Infobox aircraft image",
                AuthorParameterName = "imageauthor"
            },
            //Bundesarchiv key: BArch-image (result.InfoTemplate.ParamByKey("BArch-image"))
        };
    } 
}