using System.Collections.Generic;

public interface IImageSettings
{
    int Id { get; }
    ImageType ImageType { get; }
    IEnumerable<int> SizesSquare { get; }
    IEnumerable<int> SizesFixedWidth { get; }
    string BasePath { get; }
    string BaseDummyUrl { get; }
    string ServerPathAndId();
    string ImageFolderPath(); 

    void Init(int typeId);

    void DeleteFiles();
}