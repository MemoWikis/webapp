public class CategoryRelation_EditData_V2 : CategoryRelation_EditData
{
    public CategoryRelation_EditData_V2() { }

    public CategoryRelation_EditData_V2(CategoryRelation categoryRelation)
    {
        CategoryId = categoryRelation.Category.Id;
        RelationType = categoryRelation.CategoryRelationType;
        RelatedCategoryId = (int)categoryRelation.RelatedCategory.Id;
    }
}