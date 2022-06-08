/// <summary>
/// Eine abstrakte Basisklasse f�r Archivdaten von Beziehungen von Themen. Ab V2 zu verwenden.
/// Ab CategoryRelation_EditData_V2 wird die Zielkategorie (d.h. auf die verwiesen wird)
/// korrekterweise gelesen/ geschrieben, ohne die vorher (dh in V1) eine Anzeige von verwandten
/// Kategorien nicht m�glich war.
/// </summary>
public abstract class CategoryRelation_EditData
{
    public int CategoryId;
    public int RelatedCategoryId;
}