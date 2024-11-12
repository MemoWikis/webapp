public class PageRelationEditDataV1 : PageRelation_EditData
{
    public PageRelationEditDataV1() { }

    public PageRelationEditDataV1(PageRelation? pageRelation)
    {
        if (pageRelation != null)
        {
            PageId = pageRelation.Child.Id;
            RelatedPageId = (int)pageRelation.Parent.Id;
        }
    }
}