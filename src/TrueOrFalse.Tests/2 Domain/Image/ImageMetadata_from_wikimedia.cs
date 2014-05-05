using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ObjectDumper;

namespace TrueOrFalse.Tests
{
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
            var result = Resolve<WikiImageMetaLoader>().Run("http://en.wikipedia.org/wiki/File:Solr.png");
            Assert.That(result.ApiHost, Is.EqualTo("en.wikipedia.org"));
            Assert.That(result.ImageNotFound, Is.False);
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

}