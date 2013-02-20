using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TrueOrFalse.Tests
{
    public class ImageMetadata_from_wikimedia : BaseTest
    {
        [Test]
        public void Load_image()
        {
            var result = Resolve<WikimediaImageLoader>().Run("Platichthys_flesus_Vääna-Jõesuu_in_Estonia.jpg");
        }
    }
}
