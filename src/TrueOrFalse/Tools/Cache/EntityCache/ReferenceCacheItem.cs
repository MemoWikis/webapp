using System.Collections.Generic;
using System.Linq;
using Seedworks.Lib.Persistence;

[Serializable]
public class ReferenceCacheItem : DomainEntity
{
    public virtual QuestionCacheItem Question { get; set; }
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
    public static IEnumerable<ReferenceCacheItem> ToReferenceCacheItems(IList<Reference> references) =>
        references.Select(reference => ToReferenceCacheItem(reference));
    public static ReferenceCacheItem ToReferenceCacheItem(Reference reference)
    {
        if (reference.Category != null)
            return new ReferenceCacheItem()
            {
                Question = EntityCache.GetQuestion(reference.Question.Id),
                Category = EntityCache.GetCategory(reference.Category.Id) ?? new CategoryCacheItem("Empty"),
                ReferenceType = reference.ReferenceType,
                AdditionalInfo = reference.AdditionalInfo,
                ReferenceText = reference.ReferenceText
            };

        return new ReferenceCacheItem()
        {
            Question = EntityCache.GetQuestion(reference.Question.Id),
            ReferenceType = reference.ReferenceType,
            AdditionalInfo = reference.AdditionalInfo,
            ReferenceText = reference.ReferenceText
        };
    }
}

