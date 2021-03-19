public class CategoryRelation_EditData_V2 : CategoryRelation_EditData
{
    public CategoryRelation_EditData_V2() { }

    public CategoryRelation_EditData_V2(CategoryRelation categoryRelation)
    {
        CategoryId = categoryRelation.CategoryId.Id;
        RelationType = categoryRelation.CategoryRelationType;
        RelatedCategoryId = (int)categoryRelation.RelatedCategoryId.Id;
    }
}