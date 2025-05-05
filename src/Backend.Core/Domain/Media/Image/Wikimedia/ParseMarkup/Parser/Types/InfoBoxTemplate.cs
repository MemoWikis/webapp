public class InfoBoxTemplate
{
    public string TemplateName;
    public string DescriptionParamaterName = "Description";
    public string AuthorParameterName = "Author";
    public string AuthorParameterNameUrl = "";

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
            },
            new InfoBoxTemplate
            {
                TemplateName = "Flickr",
                AuthorParameterName = "photographer",
                AuthorParameterNameUrl = "photographer_url"
            },
            new InfoBoxTemplate
            {
                TemplateName = "Artwork",
                DescriptionParamaterName = "title",
            }
        };
    }
}