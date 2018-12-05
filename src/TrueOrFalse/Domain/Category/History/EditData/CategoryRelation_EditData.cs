/// <summary>
/// Eine abstrakte Basisklasse für Archivdaten von Beziehungen von Themen. Ab V2 zu verwenden.
/// Ab CategoryRelation_EditData_V2 wird die Zielkategorie (d.h. auf die verwiesen wird)
/// korrekterweise gelesen/ geschrieben, ohne die vorher (dh in V1) eine Anzeige von verwandten
/// Kategorien nicht möglich war.
/// </summary>
public abstract class CategoryRelation_EditData
{
    public CategoryRelationType RelationType;
    public int CategoryId;
    public int RelatedCategoryId;

    public CategoryRelation ToCategoryRelation()
    {
        var cat = Sl.CategoryRepo.GetById(CategoryId);
        var relatedCat = Sl.CategoryRepo.GetById(RelatedCategoryId);
        return new CategoryRelation
        {
            Category = cat,
            RelatedCategory = relatedCat,
            CategoryRelationType = RelationType
        };
    }
}