[Serializable]
public class ReferenceCacheItem : DomainEntity
{
    public virtual QuestionCacheItem Question { get; set; }
    public virtual PageCacheItem Page { get; set; }
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

    public static IEnumerable<ReferenceCacheItem>
        ToReferenceCacheItems(IList<Reference> references) =>
        references.Select(reference => ToReferenceCacheItem(reference));

    public static ReferenceCacheItem ToReferenceCacheItem(Reference reference)
    {
        if (reference.Page != null)
            return new ReferenceCacheItem()
            {
                Question = EntityCache.GetQuestion(reference.Question.Id),
                Page = EntityCache.GetPage(reference.Page.Id) ??
                           new PageCacheItem("Empty"),
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