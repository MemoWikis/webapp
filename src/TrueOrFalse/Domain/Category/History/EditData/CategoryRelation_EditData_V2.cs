public class CategoryRelation_EditData_V2 : CategoryRelation_EditData
{
    public CategoryRelation_EditData_V2() { }

    public CategoryRelation_EditData_V2(CategoryRelation categoryRelation)
    {
        CategoryId = categoryRelation.Child.Id;
        RelatedCategoryId = (int)categoryRelation.Parent.Id;
    }
}