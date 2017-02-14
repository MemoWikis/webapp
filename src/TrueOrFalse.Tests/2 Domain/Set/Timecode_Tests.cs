using NUnit.Framework;

public class Timecode_tests
{
    [Test]
    public void Should_parse_timecode()
    {
        Assert.That(Timecode.ToSeconds("11"), Is.EqualTo(11));
        Assert.That(Timecode.ToSeconds("0:11"), Is.EqualTo(11));
        Assert.That(Timecode.ToSeconds("1:11"), Is.EqualTo(71));
        Assert.That(Timecode.ToSeconds("4f"), Is.EqualTo(4));
        Assert.That(Timecode.ToSeconds("---"), Is.EqualTo(0));
    }

    [Test]
    public void Should_nicly_format_timecode()
    {
        Assert.That(Timecode.ToString(11), Is.EqualTo("0:11"));
        Assert.That(Timecode.ToString(71), Is.EqualTo("1:11"));
        Assert.That(Timecode.ToString(0), Is.EqualTo("0:00"));
    }
}