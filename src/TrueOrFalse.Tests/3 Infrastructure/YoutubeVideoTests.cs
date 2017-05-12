using NUnit.Framework;

[TestFixture]
public class YoutubeVideoTests
{
    [Test]
    public void GetVideoKey()
    {
        Assert.That(YoutubeVideo.GetVideoKeyFromUrl("https://www.youtube.com/watch?v=KZp15FYS_ds"), Is.EqualTo("KZp15FYS_ds"));
        Assert.That(YoutubeVideo.GetVideoKeyFromUrl("https://www.youtube.com/"), Is.EqualTo(""));
        Assert.That(YoutubeVideo.GetVideoKeyFromUrl(null), Is.EqualTo(""));
        Assert.That(YoutubeVideo.GetVideoKeyFromUrl(""), Is.EqualTo(""));
        Assert.That(YoutubeVideo.GetVideoKeyFromUrl("https://www.youtube.com/watch?v=BKZsXZDH438&feature=youtu.be"), Is.EqualTo("BKZsXZDH438"));
    }
}