using System.Collections.Generic;

public interface IImageSettings
{
    int Id { get; }
    IEnumerable<int> SizesSquare { get; }
    IEnumerable<int> SizesFixedWidth { get; }
    string BasePath { get; }
    string ServerPathAndId();
}