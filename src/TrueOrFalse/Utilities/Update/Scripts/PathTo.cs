namespace TrueOrFalse.Updates
{
    public class PathTo
    {
        public static string Scrips(string fileName)
        {
            return "Utilities/Update/Scripts/" + fileName;
        }

        public static string SolrSchema(string fileName)
        {
            return "Utilities/Update/SolrSchemas/" + fileName;
        }
    }
}
