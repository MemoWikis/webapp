public class PageRelationEditDataV1 : PageRelation_EditData
{
    public PageRelationEditDataV1() { }

    public PageRelationEditDataV1(PageRelation? categoryRelation)
    {
        if (categoryRelation != null)
        {
            PageId = categoryRelation.Child.Id;
            RelatedPageId = (int)categoryRelation.Parent.Id;
        }
    }
}