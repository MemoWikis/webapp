using System.Collections.Generic;

public interface IImageSettings
{
    IEnumerable<int> SizesSquare { get; }
    IEnumerable<int> SizesFixedWidth { get; }
    string BasePath { get; }
    string BasePathAndId();
}