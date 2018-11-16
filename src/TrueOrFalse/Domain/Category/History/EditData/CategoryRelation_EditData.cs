/// <summary>
/// Ab CategoryRelation_EditData_V2 wird die Zielkategorie (d.h. auf die verwiesen wird)
/// korrekterweise gelesen/ geschrieben, ohne die vorher (dh in V1) eine Anzeige von verwandten
/// Kategorien nicht möglich war.
/// </summary>
public abstract class CategoryRelation_EditData
{
    public int RelationType;
    public int CategoryId;
    public int RelatedCategoryId;
}