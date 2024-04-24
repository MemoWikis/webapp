using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;

public class StoreSound
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public StoreSound(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public void Run(Stream inputStream, string filename)
    {
        File.Delete(filename);

        var tempFileName = Path.GetTempFileName();
        using (var savedFile = File.OpenWrite(tempFileName))
        {
            inputStream.CopyTo(savedFile);
        }

        var ffmpegPath = Path.Combine(_webHostEnvironment.WebRootPath, "/bin/ffmpeg.exe");
        if (!File.Exists(ffmpegPath))
            throw new FileNotFoundException(string.Format(
                "Please copy ffmpeg.exe from http://ffmpeg.zeranoe.com/builds/ to {0}.",
                ffmpegPath));
        var ffmpeg = Process.Start(ffmpegPath, string.Format("-i {0} {1}", tempFileName, filename));
        if (ffmpeg != null) ffmpeg.WaitForExit();

        File.Delete(tempFileName);
    }
}