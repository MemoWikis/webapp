using NUnit.Framework;
using TrueOrFalse;

public class ImageMetadata_from_wikimedia : BaseTest
{
    [Test]
    public void Known_image_should_contain_meta_data()
    {
        var result1 = Resolve<WikiImageMetaLoader>().Run("Platichthys_flesus_Vääna-Jõesuu_in_Estonia.jpg");
        Assert.That(result1.ImageNotFound, Is.False);
        Assert.That(result1.ImageTitle, Is.EqualTo("File:Platichthys flesus Vääna-Jõesuu in Estonia.jpg"));

        var result2 = Resolve<WikiImageMetaLoader>().Run("http://commons.wikimedia.org/wiki/File:Berlin_relief_location_map-names.png");
        Assert.That(result2.ImageNotFound, Is.False);
        Assert.That(result2.ImageTitle, Is.EqualTo("File:Berlin relief location map-names.png"));

        var result3 = Resolve<WikiImageMetaLoader>().Run("Platichthys_flesus_Vääna-Jõesuu_in_Estonia.jpg?lang=de");
        Assert.That(result3.ImageNotFound, Is.False);
        Assert.That(result3.ImageTitle, Is.EqualTo("File:Platichthys flesus Vääna-Jõesuu in Estonia.jpg"));

        var result4 = Resolve<WikiImageMetaLoader>().Run("http://commons.wikimedia.org/wiki/Hauptseite?uselang=de#mediaviewer/File:Liguus_virgineus_01.JPG");
        Assert.That(result4.ImageNotFound, Is.False);
        Assert.That(result4.ImageTitle, Is.EqualTo("File:Liguus virgineus 01.JPG"));

        var result5 = Resolve<WikiImageMetaLoader>().Run("http://commons.wikimedia.org/wiki/File:Liguus_virgineus_01.JPG?uselang=de");
        Assert.That(result5.ImageNotFound, Is.False);
        Assert.That(result5.ImageTitle, Is.EqualTo("File:Liguus virgineus 01.JPG"));

        var result6 = Resolve<WikiImageMetaLoader>().Run("http://upload.wikimedia.org/wikipedia/commons/0/02/Liguus_virgineus_01.JPG");
        Assert.That(result6.ImageNotFound, Is.False);
        Assert.That(result6.ImageTitle, Is.EqualTo("File:Liguus virgineus 01.JPG"));

        var result7 = Resolve<WikiImageMetaLoader>().Run("http://upload.wikimedia.org/wikipedia/commons/0/02/Liguus_virgineus_01.JPG?uselang=de");
        Assert.That(result7.ImageNotFound, Is.False);
        Assert.That(result7.ImageTitle, Is.EqualTo("File:Liguus virgineus 01.JPG"));

        var result8 = Resolve<WikiImageMetaLoader>().Run("Liguus virgineus 01.JPG");
        Assert.That(result8.ImageNotFound, Is.False);
        Assert.That(result8.ImageTitle, Is.EqualTo("File:Liguus virgineus 01.JPG"));
    }

    [Test]
    public void Should_get_for_svgs_png_url()
    {
        var result1 = Resolve<WikiImageMetaLoader>().Run("SVG_logo.svg");
        Assert.That(result1.ImageNotFound, Is.False);
        Assert.That(result1.ImageTitle, Is.EqualTo("File:SVG logo.svg"));
        Assert.That(result1.ImageUrl, Is.StringEnding("png"));

        var result2 = Resolve<WikiImageMetaLoader>().Run("Bundesministerium_f%C3%BCr_Wirtschaft_und_Energie_Logo.svg");
        Assert.That(result2.ImageNotFound, Is.False);
        Assert.That(result2.ImageTitle, Is.EqualTo("File:Bundesministerium für Wirtschaft und Energie Logo.svg"));

        var result3 = Resolve<WikiImageMetaLoader>().Run("http://de.wikipedia.org/wiki/Datei:Bundesministerium_f%C3%BCr_Wirtschaft_und_Energie_Logo.svg");
        Assert.That(result3.ImageNotFound, Is.False);
        Assert.That(result3.ImageTitle, Is.EqualTo("File:Bundesministerium für Wirtschaft und Energie Logo.svg"));
    }

    [Test]
    public void Image_not_found_should_be_indicated()
    {
        Assert.That(
            Resolve<WikiImageMetaLoader>().Run("not-existing-image-9812.jpg").ImageNotFound,
            Is.True
        );
    }

    [Test]
    public void If_image_not_on_commons_try_original_host()
    {
        var result1 = Resolve<WikiImageMetaLoader>().Run("http://en.wikipedia.org/wiki/File:Solr.png");
        Assert.That(result1.ApiHost, Is.EqualTo("en.wikipedia.org"));
        Assert.That(result1.ImageNotFound, Is.False);

        var result2 = Resolve<WikiImageMetaLoader>().Run("https://de.wikipedia.org/wiki/James_Bond#/media/File:007_evolution.svg");
        Assert.That(result2.ApiHost, Is.EqualTo("en.wikipedia.org"));
        Assert.That(result2.ImageNotFound, Is.False);
    }

    [Test]
    public void Should_extract_host()
    {
        Assert.That(WikiApiUtils.ExtractDomain("http://en.wikipedia.org/wiki/File:Solr.png"), Is.EqualTo("en.wikipedia.org"));
        Assert.That(WikiApiUtils.ExtractDomain("en.wikipedia.org/wiki/File:Solr.png"), Is.EqualTo("en.wikipedia.org"));
        Assert.That(WikiApiUtils.ExtractDomain("someFoo/"), Is.EqualTo(null));
        Assert.That(WikiApiUtils.ExtractDomain("http://someFoo/"), Is.EqualTo(null));
        Assert.That(WikiApiUtils.ExtractDomain("not-existing-image-9812.jpg"), Is.EqualTo(null));
    }
}