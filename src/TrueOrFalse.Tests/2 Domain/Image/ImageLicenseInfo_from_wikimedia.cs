using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using TrueOrFalse.Tests;
using TrueOrFalse.WikiMarkup;


namespace TrueOrFalse.Tests._2_Domain.Image
{
    internal class ImageLicenseInfo_from_Wikimedia : BaseTest
    {
        [Test]
        public void Get_license_info()
        {
            var licenseInfoLoader = Resolve<WikiImageLicenseLoader>();
            var licenseInfo = licenseInfoLoader.Run("Platichthys_flesus_Vääna-Jõesuu_in_Estonia.jpg", "commons.wikimedia.org");
            Assert.That(licenseInfo.AuthorName, Is.EqualTo("Tiit Hunt"));
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
                Is.EqualTo(
                    "Eine [[:de:Flunder|Flunder]], ''Platichthys flesus'', nahe dem estnischen Dorf [[:de:Vääna-Jõesuu|Vääna-Jõesuu]]"));
            Assert.That(parsedImageMakup.Description,
                Is.EqualTo("Eine Flunder, <i>Platichthys flesus</i>, nahe dem estnischen Dorf Vääna-Jõesuu"));

            Assert.That(parsedImageMakup.AuthorName_Raw, Is.EqualTo("[[User:Tiithunt|Tiit Hunt]]"));
            Assert.That(parsedImageMakup.AuthorName, Is.EqualTo("Tiit Hunt"));

            Assert.That(parsedImageMakup.LicenseIsCreativeCommons, Is.EqualTo(true));
            Assert.That(parsedImageMakup.LicenseTemplateString, Is.EqualTo("cc-by-sa-3.0"));
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

        [Test]
        public void Should_parse_de()
        {
            const string demoText = @"== {{int:filedesc}} ==
                {{Information
                |Description    ={{de|1=Rita Süssmuth auf der 50. Verleihung der Adolf-Grimme-Preise in Marl am 4. April 2014.}}
                |Source         ={{own}}
                |Author         =[[User:Michael-schilling|Michael Schilling]]
                |Date           =2014-04-04
                |Permission     =
                |other_versions =
                }}

                [[Category:Rita Süssmuth]]
                [[Category:Grimme-Preis 2014]]
                == {{int:license-header}} ==
                {{self|cc-by-sa-3.0}}            
            }";

            var parsedImageMakup = ParseImageMarkup.Run(demoText);
            Assert.That(parsedImageMakup.DescriptionDE_Raw, Is.EqualTo("Rita Süssmuth auf der 50. Verleihung der Adolf-Grimme-Preise in Marl am 4. April 2014."));
        }

        [Test]
        public void Should_parse_licenses()
        {
            const string markup = @"=={{int:filedesc}}==
                            {{Information
                            |description=
                            {{en|1=Dalian, Liaoning, China: Two elderly Chinese guys enjoying the sea at  Xinghai Bay}}
                            {{fr|1=Deux chinois âgés regardant la mer dans la baie de Xinghai à [[:fr:Dalian|Dalian]], province du Liaoning, en Chine.}}
                            {{zh|1=两个中国老人坐在星海湾码头
                            }}
                            |date=2009-05-21
                            |source={{own}}
                            |author=[[User:Cccefalon|CEphoto, Uwe Aranas]]
                            |permission={{User:Cccefalon/permission}}
                            |other_versions=
                            |other_fields={{User:Cccefalon/attribution}}
                            }}
                            {{Object location dec|38.875052|121.584854|region:CN-21}}
                            {{User:Cccefalon/no-new-version}}
                            {{Assessments|featured=1}}
                            {{picture of the day|year=2014|month=10|day=27}}

                            =={{int:license-header}}==
                            {{self|cc-by-sa-3.0|GFDL|attribution={{User:Cccefalon/attribution1}} }}

                            [[Category:Dalian]]
                            [[Category:Images by Cccefalon]]
                            [[Category:Quality images by Cccefalon]]
                            [[Category:Quality images of people by User:Cccefalon]]
                            [[Category:Quality images of China]]
                            [[Category:People of Dalian]]
                            [[Category:People of Liaoning]]
                            [[Category:Old people of China]]
                            [[Category:Quality images of China by User:Cccefalon]]
                            [[Category:Images of China by User:Cccefalon]]
                            [[Category:Images of people by User:Cccefalon]]
                            [[Category:Featured pictures by Cccefalon]]
                            [[Category:Featured pictures of people]]
                            [[Category:Featured pictures of China]]
                            {{QualityImage}}";

            Assert.That(LicenseRepository.GetAll().Any(registeredLicense => !String.IsNullOrEmpty(registeredLicense.WikiSearchString) && registeredLicense.WikiSearchString.ToLower() == "cc-by-sa-3.0"), Is.True, "GetAll failed");
            Assert.That(LicenseParser.GetApplicableLicenses(markup).Any(parsedLicense => parsedLicense.WikiSearchString.ToLower() == "cc-by-sa-3.0"), Is.True,"GetApplicable failed");
        }

        [Test]
        public void Should_find_main_license()
        {

            var sortedLicenses = new List<License>()
            {
                new License {WikiSearchString = "other"},
                new License {WikiSearchString = "gfdl"},
                new License {WikiSearchString = "cc-by-sa-3.0"},
                new License {WikiSearchString = "pd"},
                new License {WikiSearchString = "cc-by-sa-3.0-fr"},
                new License {WikiSearchString = "cc-sa-3.0"},
                new License {WikiSearchString = "cc-by-sa-2.5"},
                new License {WikiSearchString = "cc-by-sa-3.0-de"},
                new License {WikiSearchString = "alternative"},

            };

            sortedLicenses = LicenseParser.SortLicenses(sortedLicenses);

            var sortedLicenseStrings = sortedLicenses.Select(l => l.WikiSearchString).Aggregate((a, b) => a + ", " + b);
            const string expectedResult = "cc-sa-3.0, cc-by-sa-3.0, cc-by-sa-3.0-de, cc-by-sa-3.0-fr, cc-by-sa-2.5, pd, gfdl, alternative, other";

            Assert.That(sortedLicenseStrings.Equals(expectedResult),
                        String.Format("expected: {0}" + Environment.NewLine + "was: {1}", expectedResult, sortedLicenseStrings));
        }

        [Test]
        public void Should_find_other_possible_license_strings()
        {
            //$todo: refine together with tested method
            const string markup = "cc-by-sa-3.0|cc-by-sa-9.0|pd-phantasie|pd|lkjlkjlj";

            var otherPossibleLicenseStrings = LicenseParser.GetOtherPossibleLicenseStrings(markup).Aggregate((a, b) => a + ", " + b);
            const string expectedResult = "cc-by-sa-9.0, pd-phantasie";

            Assert.That(otherPossibleLicenseStrings.Equals(expectedResult),
                        String.Format("expected: {0}" + Environment.NewLine + "was: {1}", expectedResult, otherPossibleLicenseStrings));
        }

        [Test]
        public void TempFillImageData()
        {   
            //Later RunWikiMedia/StoreWiki should be tested instead
            const string markup = @"== {{int:license-header}} ==
                                    {{self|GFDL|cc-by-sa-3.0,2.5,2.0,1.0|cc-by-sa-3.0}}
                                    {{svg|sport}}";

            var mainLicense = LicenseParser.GetMainLicenseId(markup);
            var allRegisteredLicenses = string.Join(",", LicenseParser.GetAllLicenses(markup).Select(x => x.Id.ToString()));

            Console.WriteLine("Main license id: " + mainLicense);
            Console.WriteLine("Ids: " + allRegisteredLicenses);

        }

        [Test]
        public void Registered_licenses_should_not_contain_duplicates()
        {
            var duplicateIdLicenses =
                LicenseRepository.GetAll()
                    .GroupBy(l => l.Id)
                    .Where(grp => grp.Count() > 1)
                    .SelectMany(grp => grp)
                    .ToList();

            var duplicateSearchStringLicenses =
                LicenseRepository.GetAll()
                    .GroupBy(l => l.WikiSearchString)
                    .Where(grp => grp.Count() > 1)
                    .SelectMany(grp => grp)
                    .ToList();

            var messageDuplicateSearchStrings = "Duplicate search strings: " + Environment.NewLine;

            foreach (var duplicateSearchStringLicense in duplicateSearchStringLicenses)
            {
                messageDuplicateSearchStrings += String.Format("{0} (Id: {1})" + Environment.NewLine,
                                                    duplicateSearchStringLicense.WikiSearchString,
                                                    duplicateSearchStringLicense.Id);
            }
            
            Assert.That(
                !LicenseRepository.GetAll().GroupBy(l => l.Id).Any(grp => grp.Count() > 1),
                "Duplicate ID(s):" + Environment.NewLine + String.Join(Environment.NewLine, duplicateIdLicenses.Select(license => license.Id).ToList()));

            Assert.That(
                !LicenseRepository.GetAll().GroupBy(l => l.WikiSearchString).Any(grp => grp.Count() > 1),
                messageDuplicateSearchStrings);
        }
    }
}