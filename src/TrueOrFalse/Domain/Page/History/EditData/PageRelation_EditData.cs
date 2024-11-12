/// <summary>
/// Eine abstrakte Basisklasse für Archivdaten von Beziehungen von Themen. Ab V2 zu verwenden.
/// Ab CategoryRelation_EditData_V2 wird die Zielkategorie (d.h. auf die verwiesen wird)
/// korrekterweise gelesen/ geschrieben, ohne die vorher (dh in V1) eine Anzeige von verwandten
/// Kategorien nicht möglich war.
/// </summary>
public abstract class PageRelation_EditData
{
    public int PageId;
    public int RelatedPageId;
}