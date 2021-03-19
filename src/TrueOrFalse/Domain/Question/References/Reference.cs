using System;
using Seedworks.Lib.Persistence;

[Serializable]
public class Reference : DomainEntity
{
    public virtual Question Question { get; set; }
    public virtual CategoryCacheItem Category { get; set; }
    public virtual ReferenceType ReferenceType { get; set; }
    public virtual string AdditionalInfo { get; set; }
    public virtual string ReferenceText { get; set; }

    public static ReferenceType GetReferenceType(string referenceTypeString)
    {
        if (referenceTypeString == "MediaCategoryReference")
            return ReferenceType.MediaCategoryReference;

        if (referenceTypeString == "FreeTextReference")
            return ReferenceType.FreeTextreference;

        if (referenceTypeString == "UrlReference")
            return ReferenceType.UrlReference;

        throw new Exception("invalid type");

    }
}