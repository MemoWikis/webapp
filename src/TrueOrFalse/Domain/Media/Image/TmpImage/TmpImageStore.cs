using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;

/// <summary>
/// TmpImageStore is per user and stored in <see cref="SessionUiData"/> . 
/// </summary>
[Serializable]
public class TmpImageStore
{
    private readonly List<TmpImage> _tmpImages = new(); 

    public TmpImage Add(Stream inputStream, int previewWidth, IWebHostEnvironment webHostEnvironment, Logg logg)
    {
        var tmpImage = new TmpImage(previewWidth, webHostEnvironment);
        SaveImageToFile.Run(inputStream, tmpImage);

        _tmpImages.Add(tmpImage);

        return tmpImage;
    }

    public TmpImage ByGuid(string guid)
    {
        return _tmpImages.Find(x => x.Guid == guid);
    }
}