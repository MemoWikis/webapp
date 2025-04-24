public class PageRelation_EditData_V2 : PageRelation_EditData
{
    public PageRelation_EditData_V2() { }

    public PageRelation_EditData_V2(PageRelation pageRelation)
    {
        PageId = pageRelation.Child.Id;
        RelatedPageId = (int)pageRelation.Parent.Id;
    }
}