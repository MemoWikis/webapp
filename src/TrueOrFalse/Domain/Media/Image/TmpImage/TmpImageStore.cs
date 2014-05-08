using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueOrFalse.Web.Context;

/// <summary>
/// TmpImageStore is per user and stored in <see cref="SessionUiData"/> . 
/// </summary>
[Serializable]
public class TmpImageStore
{
    private readonly List<TmpImage> _tmpImages = new List<TmpImage>(); 

    public TmpImage Add(Stream inputStream, int previewWidth)
    {
        var tmpImage = new TmpImage(previewWidth);
        StoreImages.Run(inputStream, tmpImage);

        _tmpImages.Add(tmpImage);

        return tmpImage;
    }

    public TmpImage ByGuid(string guid)
    {
        return _tmpImages.Find(x => x.Guid == guid);
    }
}