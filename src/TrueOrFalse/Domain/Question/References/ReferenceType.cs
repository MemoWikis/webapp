public enum ReferenceType
{
    MediaCategoryReference = 1,
    FreeTextreference = 2,
    UrlReference = 3,
}

public static class ReferenceTypeExt
{

    public static string GetName(this ReferenceType e)
    {
        switch (e)
        {
            case ReferenceType.MediaCategoryReference:
                return "MediaCategoryReference";

            case ReferenceType.FreeTextreference:
                return "FreeTextReference";

            case ReferenceType.UrlReference:
                return "UrlReference";
        }

        throw new Exception("invalid reference type");
    }
}