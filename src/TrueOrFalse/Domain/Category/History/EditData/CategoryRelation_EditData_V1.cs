public class CategoryRelation_EditData_V1 : CategoryRelation_EditData
{
    public int RelationType;
    public int CategoryId;

    public CategoryRelation_EditData_V1() { }

    public CategoryRelation_EditData_V1(CategoryRelation categoryRelation)
    {
        CategoryId = categoryRelation.Category.Id;
        RelationType = (int)categoryRelation.CategoryRelationType;
    }
}