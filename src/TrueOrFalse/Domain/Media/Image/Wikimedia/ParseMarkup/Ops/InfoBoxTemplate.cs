﻿using System.Collections.Generic;

public class InfoBoxTemplate
{
    public string TemplateName;
    public string DescriptionParamaterName = "Description";
    public string AuthorParameterName = "Author";

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
            new InfoBoxTemplate
            {
                TemplateName = "BArch-image",
                AuthorParameterName = "photographer"
            }
        };
    } 
}