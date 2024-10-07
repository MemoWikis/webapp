public class CategoryRelation_EditData_V1 : CategoryRelation_EditData
{
    public CategoryRelation_EditData_V1() { }

    public CategoryRelation_EditData_V1(CategoryRelation? categoryRelation)
    {
        if (categoryRelation != null)
        {
            CategoryId = categoryRelation.Child.Id;
            RelatedCategoryId = (int)categoryRelation.Parent.Id;
        }
    }
}