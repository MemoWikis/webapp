using System;
using System.Linq;
using NUnit.Framework;
using TrueOrFalse.Tests;

[TestFixture]
public class YoutubeVideoTests
{
    [Test]
    public void GetVideoKey()
    {
 //       Assert.That(YoutubeVideo.GetVideoKeyFromUrl("https://www.youtube.com/watch?v=KZp15FYS_ds"), Is.EqualTo("KZp15FYS_ds"));
        Assert.That(YoutubeVideo.GetVideoKeyFromUrl("https://www.youtube.com/"), Is.EqualTo(""));
        Assert.That(YoutubeVideo.GetVideoKeyFromUrl(null), Is.EqualTo(""));
        Assert.That(YoutubeVideo.GetVideoKeyFromUrl(""), Is.EqualTo(""));
        Assert.That(YoutubeVideo.GetVideoKeyFromUrl("https://www.youtube.com/watch?v=BKZsXZDH438&feature=youtu.be"), Is.EqualTo("BKZsXZDH438"));
    }

    [Test]
    public void Show_youtube_image_preview_for_sets_with_no_video()
    {
        var set = ContextSet.New().AddSet("Set").Persist().All.First();
        set.VideoUrl = "https://www.youtube.com/watch?v=KZp15FYS_ds";
        Sl.SetRepo.Update(set);

        var imageSettings = new SetImageSettings(set.Id);

        Assert.That(imageSettings.GetUrl_128px_square().Url, Is.EqualTo("https://img.youtube.com/vi/KZp15FYS_ds/0.jpg"));
    }
}