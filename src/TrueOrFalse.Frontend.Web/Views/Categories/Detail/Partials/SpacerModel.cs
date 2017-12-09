public class SpacerModel : BaseModel
{
    public int AmountSpaces;
    public bool AddBorderTop;

    public SpacerModel(int amountSpaces = 0, bool addBorderTop = false)
    {
        AmountSpaces = amountSpaces < 1 ? 2 : amountSpaces;
        AddBorderTop = addBorderTop;
    }
}
