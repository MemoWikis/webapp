public class SpacerModel : BaseContentModule
{
    public int AmountSpaces;
    public bool AddBorderTop;

    public SpacerModel(SpacerJson spacerJson)
    {
        AmountSpaces = spacerJson.AmountSpaces < 1 ? 2 : spacerJson.AmountSpaces;
        AddBorderTop = spacerJson.AddBorderTop;
    }
}
