using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}