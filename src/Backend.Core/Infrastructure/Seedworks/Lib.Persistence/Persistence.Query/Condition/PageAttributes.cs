namespace Seedworks.Lib.Persistence;

[Serializable]
public enum Importance
{
    None = 0,
    Prominent = 1
}

public interface IPageNumeric
{
    object Value { get; }
}

[Serializable]
public abstract class PageBaseAttribute : Attribute
{
}

[Serializable]
public class PageBooleanAttribute : PageBaseAttribute
{
}

[Serializable]
public class PageIntegerAttribute : PageBaseAttribute, IPageNumeric
{
    public int Value { get; private set; }

    object IPageNumeric.Value
    {
        get { return Value; }
    }

    public PageIntegerAttribute(int value)
    {
        Value = value;
    }
}

[Serializable]
public class PageSingleAttribute : PageBaseAttribute, IPageNumeric
{
    public Single Value { get; private set; }

    object IPageNumeric.Value
    {
        get { return Value; }
    }

    public PageSingleAttribute(Single value)
    {
        Value = value;
    }
}

[Serializable]
public class PageDoubleAttribute : PageBaseAttribute, IPageNumeric
{
    public double Value { get; private set; }

    object IPageNumeric.Value
    {
        get { return Value; }
    }

    public PageDoubleAttribute(double value)
    {
        Value = value;
    }
}