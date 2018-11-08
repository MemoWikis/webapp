public class CategoryRelation_EditData_V2
{
    public int RelationType;
    public int CategoryId;
    public int RelatedCategoryId;

    public CategoryRelation_EditData_V2() { }

    public CategoryRelation_EditData_V2(CategoryRelation categoryRelation)
    {
        CategoryId = categoryRelation.Category.Id;
        RelationType = (int)categoryRelation.CategoryRelationType;
        RelatedCategoryId = (int)categoryRelation.RelatedCategory.Id;
    }
}