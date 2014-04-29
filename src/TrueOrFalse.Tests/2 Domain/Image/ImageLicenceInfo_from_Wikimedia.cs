using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;
using TrueOrFalse.Tests;
using TrueOrFalse.WikiMarkup;


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
            Assert.That(licenceInfo.AuthorName, 
                Is.EqualTo("By Tiit Hunt (Own work) [CC-BY-SA-3.0 (http://creativecommons.org/licenses/by-sa/3.0)], via Wikimedia Commons"));

        }

        [Test]
        public void Should_parse_markup_of_image_details_page()
        {
            const string demoText = @"
                {{Information
                |Description    = {{mld
                | cs = [[:cs:Platýs bradavičnatý|Platýs bradavičnatý]] blízko estonské vesnice Vääna Jöesuu
                | de = Eine [[:de:Flunder|Flunder]], ''Platichthys flesus'', nahe dem estnischen Dorf [[:de:Vääna-Jõesuu|Vääna-Jõesuu]]
                | en = ''{{W|European flounder|Platichthys flesus}}'' near {{W|Vääna-Jõesuu}} in [[:w:Estonia|Estonia]].
                | et = Vääna-Jõesuu rannikumere lest
                | fr = Flet commun, ''Platichthys flesus'', près de Vääna-Jöesuu en [[:Estonie|Estonie]].
                | pt = [[:pt:Linguado|Linguado]], ''Platichthys flesus'', nas proximidades de {{W|Vääna-Jõesuu}} na [[:w:Estonia|Estônia]].
                }}
                |Date           = 2010-01-06
                |Source         = {{own}}
                |Author         = [[User:Tiithunt|Tiit Hunt]]
                |Permission     = 
                |Other versions = 
                |Other fields   = 
                }}
                {{HELP 2011}}
                {{Assessments|featured=1|com-nom=Lest.jpg}}
                {{picture of the day|year=2013|month=02|day=18}}
                {{QualityImage}}

                =={{int:license-header}}==
                {{self|cc-by-sa-3.0}}

                [[Category:Platichthys flesus]]
                [[Category:Fish of Estonia]]
                [[Category:Vääna-Jõesuu]]
                [[Category:Unassessed QI candidates]]
                [[Category:Featured pictures of Estonia]]
                [[Category:Featured pictures supported by Wikimedia Eesti]]
                [[Category:Taken with Canon EOS 5D Mark II]]
                [[Category:Quality images of animals in Estonia]]
                [[Category:Uploaded with UploadWizard]]";


            var parsedImageMakup = ParseImageMarkup.Run(demoText);
            var infoSectionParams = parsedImageMakup.InfoTemplate.Parameters;

            Assert.That(infoSectionParams.Count, Is.EqualTo(7));
            Assert.That(infoSectionParams[0].Key, Is.EqualTo("Description"));
            Assert.That(infoSectionParams[1].Key, Is.EqualTo("Date"));
            Assert.That(infoSectionParams[1].Value, Is.EqualTo("2010-01-06"));

            Assert.That(parsedImageMakup.DescriptionDE_Raw, 
                Is.EqualTo("Eine [[:de:Flunder|Flunder]], ''Platichthys flesus'', nahe dem estnischen Dorf [[:de:Vääna-Jõesuu|Vääna-Jõesuu]]"));
            Assert.That(parsedImageMakup.Description, Is.EqualTo("Eine Flunder, <i>Platichthys flesus</i>, nahe dem estnischen Dorf Vääna-Jõesuu"));

            Assert.That(parsedImageMakup.AuthorName_Raw, Is.EqualTo("[[User:Tiithunt|Tiit Hunt]]"));
            Assert.That(parsedImageMakup.AuthorName, Is.EqualTo("Tiit Hunt"));

            Assert.That(parsedImageMakup.LicenceIsCreativeCommons, Is.EqualTo(true));
            Assert.That(parsedImageMakup.LicenceTemplateString, Is.EqualTo("cc-by-sa-3.0"));
        }

        [Test]
        public void Should_parse_markup()
        {
            const string demoText = @"
                == {{int:filedesc}} ==
                {{Uploaded with en.wp UW marker|year=2013|month=01|day=16}}
                {{Information
                |Description = {{en|A source code example that shows classes, methods, and inheritance.
                This is NOT THE Mint Programming Language because Mint absolutely cannot perform a 'return this' implicitly at the end of a particular function.}}
                |Source = Taking a screenshot, then editing using Paint.NET
                |Date = 2013-01-16
                |Author = [[User:Carrot Lord|Carrot Lord]]
                }}

                == {{int:license-header}} ==
                {{self|GFDL|cc-by-sa-3.0|migration=redundant}}

                [[Category:Programming languages]]";

            var parsedImageMakup = ParseImageMarkup.Run(demoText);
            Assert.That(parsedImageMakup.AuthorName_Raw, Is.EqualTo("[[User:Carrot Lord|Carrot Lord]]"));
            Assert.That(parsedImageMakup.AuthorName, Is.EqualTo("Carrot Lord"));
        }

    }
}
