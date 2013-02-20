using System.Diagnostics;
using System.IO;
using System.Web;

public class StoreSound
{
    public void Run(Stream inputStream, string filename)
    {
        File.Delete(filename);

        var tempFileName = Path.GetTempFileName();
        using (var savedFile = File.OpenWrite(tempFileName))
        {
            inputStream.CopyTo(savedFile);
        }

        var ffmpegPath = HttpContext.Current.Server.MapPath("/bin/ffmpeg.exe");
        if (!File.Exists(ffmpegPath)) throw new FileNotFoundException(string.Format("Please copy ffmpeg.exe from http://ffmpeg.zeranoe.com/builds/ to {0}.", ffmpegPath));
        var ffmpeg = Process.Start(ffmpegPath, string.Format("-i {0} {1}", tempFileName, filename));
        if (ffmpeg != null) ffmpeg.WaitForExit();

        File.Delete(tempFileName);
    }
}