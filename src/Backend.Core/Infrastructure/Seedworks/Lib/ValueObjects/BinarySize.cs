[Serializable]
public class BinarySize
{
    public long Bytes;

    public BinarySize()
    {
    }

    public BinarySize(long length)
    {
        Bytes = length;
    }

    public static BinarySize operator +(BinarySize a, BinarySize b)
    {
        return new BinarySize(a.Bytes + b.Bytes);
    }
}