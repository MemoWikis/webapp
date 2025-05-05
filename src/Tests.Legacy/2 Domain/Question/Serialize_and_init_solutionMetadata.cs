using NUnit.Framework;

namespace TrueOrFalse.Tests;

[TestFixture]
public class Serialize_and_init_solutionMetadata : BaseTest
{
    [Test]
    public void Should_serialze_and_deserialize_MetaDataText()
    {
        var solutionMeta = new SolutionMetadataText {IsCaseSensitive = true};
        var solutionToInit = new SolutionMetadataText {Json = solutionMeta.Json};

        Assert.That(solutionToInit.IsText, Is.True);
        Assert.That(solutionToInit.IsNumber, Is.False);
        Assert.That(solutionMeta.IsCaseSensitive, Is.EqualTo(solutionToInit.IsCaseSensitive));
        Assert.That(solutionMeta.IsExactInput, Is.EqualTo(solutionToInit.IsExactInput));
        Assert.That(solutionMeta.Json, Is.EqualTo(solutionToInit.Json));
    }

    [Test]
    public void Should_serialize_type()
    {
        var solutionMeta = new SolutionMetadataDate{ Precision = DatePrecision.Month };
        var solutionMetaNew = new SolutionMetadata { Json = solutionMeta.Json };

        Assert.That(solutionMetaNew.IsDate, Is.True);
        var solutionMetaDate = solutionMetaNew.GetForDate();
        Assert.That(solutionMetaDate.Precision, Is.EqualTo(DatePrecision.Month));
    }
}