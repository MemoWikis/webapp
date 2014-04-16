using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TrueOrFalse.Tests;


namespace TrueOrFalse.Tests._2_Domain.Image
{
    class ImageLicenceInfo_from_wikimedia : BaseTest
    {
        [Ignore]//tmp
        [Test]
        public void Get_licence_info()
        {
            var licenceInfoLoader = Resolve<WikiImageLicenceLoader>();
            var licenceInfo = licenceInfoLoader.Run("Platichthys_flesus_Vääna-Jõesuu_in_Estonia.jpg");
            Assert.That(licenceInfo.Attribution, 
                Is.EqualTo("By Tiit Hunt (Own work) [CC-BY-SA-3.0 (http://creativecommons.org/licenses/by-sa/3.0)], via Wikimedia Commons"));

        }
    }
}
