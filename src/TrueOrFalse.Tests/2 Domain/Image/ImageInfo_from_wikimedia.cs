using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using SolrNet.Utils;
using TrueOrFalse.Tests;
using TrueOrFalse.WikiMarkup;


namespace TrueOrFalse.Tests._2_Domain.Image
{
    internal class ImageInfo_from_Wikimedia : BaseTest
    {
        public static string GetMarkupDemoText_mld_template()
        {
            return @"
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
        }

        public static string GetMarkupDemoText_several_languages_with_de()
        {
            return @"{{Information
                        |Description={{en|1=View in the [[:en:Kolob Canyons|Kolob Canyons]]. Kolob Canyons is the northwest section of [[:en:Zion National Park|Zion National Park]], [[:en:Utah|Utah]], [[:en:United States|United States]]. The Kolob Canyons are part of the [[:en:Colorado Plateau|Colorado Plateau]] region of the Zion National Park and are noted for their colorful beauty and diverse landscape. This part of Zion National Park is accessed by a park road about 20 miles south of [[:en:Cedar City, Utah|Cedar City, Utah]] off [[:en:Interstate 15 in Utah|Interstate 15]].}}  
                        {{aa|1= test. Blick in den Kolob Canyons. ''Kolob Canyons'' ist der nordwestliche Teil vom [[:de:Zion National Park|Zion National Park]], [[:de:Utah|Utah]], [[:de:Vereinigte Staaten|USA]]. Er gehört zu dem hochgelegenen [[:de:Colorado-Plateau|Colorado-Plateau]] und ist bekannt geworden durch die prächtigen Farben seiner Gebirgslandschaft. Er wird trotz seiner Schönheit selten aufgesucht, weil es in dem Zion National Park keine Straße gibt, die dorthin führt. Die Straße in den ''Kolob Canyons'' beginnt an der [[:de:Interstate_15#Utah|Interstate_15]] etwa 20 Meilen südlich der Stadt [[:de:Cedar City (Utah)|Cedar City]].}}
                        {{de|1= Blick in den Kolob Canyons. ''Kolob Canyons'' ist der nordwestliche Teil vom [[:de:Zion National Park|Zion National Park]], [[:de:Utah|Utah]], [[:de:Vereinigte Staaten|USA]]. Er gehört zu dem hochgelegenen [[:de:Colorado-Plateau|Colorado-Plateau]] und ist bekannt geworden durch die prächtigen Farben seiner Gebirgslandschaft. Er wird trotz seiner Schönheit selten aufgesucht, weil es in dem Zion National Park keine Straße gibt, die dorthin führt. Die Straße in den ''Kolob Canyons'' beginnt an der [[:de:Interstate_15#Utah|Interstate_15]] etwa 20 Meilen südlich der Stadt [[:de:Cedar City (Utah)|Cedar City]].}}
                        {{fr|1= La région des [[:fr:Canyons de Kolob|canyons de Kolob]] Canyons dans le [[:fr:parc national de Zion|parc national de Zion]].}}
                        {{it|1=Panorama nei Kolob Canyon, nella zona nordoccidentale del [[:it:Parco nazionale di Zion|Parco nazionale di Zion]], [[:it:Utah|Utah]], [[:it:Stati Uniti d'America|Stati Uniti d'America]].}}
                        {{zh-hans|1=美国[[:zh:锡安国家公园|锡安国家公园]]科罗布峡谷。}}
                        |Source={{own}}, Collection [[User:Michael Gäbler|Michael Gäbler]]
                        |Author= [[User:Michael Gäbler|Michael Gäbler]]
                        |other_fields={{Credit line |Author = © [[User:Michael Gäbler|Michael Gäbler]]| Other = Wikimedia Commons |License = CC-BY-SA-3.0}}
                        |Date=1997-08
                        |Permission= 
                        |other_versions=
                        }}
                        {{Photo Information
                        | Model        = [[:en:Olympus OM-4|Olympus OM-4]]
                        | Shutter      = 
                        | Aperture     = 
                        | ISO          = [[:en:Film Speed|ASA 25]]
                        | Lens         = 
                        | Focal length = 
                        | Filter       = 
                        | Flash        = 
                        | Support      = 
                        | Film         = [[:en:Kodachrome|Kodachrome 25]] [[:en:Reversal film|Reversal film, 35 mm]], daylight. 
                        | Developer    = 
                        | Scanner      = Nikon Coolscan V ED
                        | Notes        = 
                        }}
                        {{Location|37|26|46.6|N|113|11|36.72|W|scale:400000_region:US}}
                        {{Assessments|featured=1}}
                        {{picture of the day|year=2013|month=07|day=04}}
                        =={{int:license}}== 
                        {{self|cc-by-3.0|author=[[User:Michael Gäbler|Michael Gäbler]]}}
                        [[Category:Kolob Canyons]]
                        [[Category:Photographs taken on Kodachrome 25 film]]
                        [[Category:Taken with Olympus OM-4]]
                        [[Category:August 1997 in the United States]]
                        [[Category:Featured pictures of Utah]]
                        [[Category:Featured pictures taken on film]]";
        }

        public static string GetMarkupDemoText_no_preferred_language_contained()
        {
            return @"
                    {{Information
                    |Description    = {{mld
                    | cs = [[:cs:Platýs bradavičnatý|Platýs bradavičnatý]] blízko estonské vesnice Vääna Jöesuu
                    | et = Vääna-Jõesuu rannikumere lest
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
        }

        public static string GetMarkupDemoText_description_is_plain_text()
        {
            return @"{{SupersededPNG|File:Clarinet.png}}
                    == {{int:filedesc}} ==
                    {{Information
                    |Description=Clarinet with a Boehm System.
                    |Source=[http://www.777life.com/photos/index.html] ; uploaded on [http://fr.wikipedia.org fr.wikipedia]; description page is/was [http://fr.wikipedia.org/w/index.php?title=Image%3AClarinette.jpg here].
                    |Date={{original upload date|2004-04-24}}; 2007-01-05 (last version)
                    |Author=[[:fr:User:Ratigan|Ratigan]] at [http://fr.wikipedia.org fr.wikipedia], and it was the same file as [[User:Mezzofortist|Mezzofortist]] uploaded here. Later versions were uploaded by [[:fr:User:Luna04|Luna04]], [[:fr:User:Iunity|Iunity]] at [http://fr.wikipedia.org fr.wikipedia]. This is a cropped version of Iunity's version.
                    |Permission=''unlimited free use'', so this image is in the [[public domain]].
                    |other_versions={{DerivativeVersions|Clarinet flopped.png}}
                    }}

                    == {{int:license-header}} ==
                    {{Atelier graphique}}
                    {{Copyrighted free use}}

                    == Original upload log on fr.wikipedia ==
                    (All user names refer to fr.wikipedia)
                    * 2007-01-05 17:17 [[:fr:User:Iunity|Iunity]] 300×1200×8 (25012 bytes) ''<nowiki>détourage</nowiki>''
                    * 2004-10-22 19:49 [[:fr:User:Luna04|Luna04]] 300×1200×8 (48031 bytes) ''<nowiki>clarinette - source http://www.777life.com/photos/instruments/JM_Clarinet.jpg (''unlimited free use'') - {{DomainePublic}}</nowiki>''
                    * 2004-04-24 18:55 [[:fr:User:Ratigan|Ratigan]] 603×123×8 (19113 bytes) ''<nowiki>Photo d'une clarinette</nowiki>''

                    [[Category:Clarinets]]
                    [[Category:Musical instruments on white background]]";
        }

        public string GetMarkupDemoText_special_instead_of_information_template()
        {
            return @"=={{int:filedesc}}==
                    {{Infobox aircraft image
                    |description={{en|Departing from M&#225;laga during the sunset. My first photo accepted with the Nikon D70}}
                    |aircraft=Airbus A319-111
                    |aircraftid=G-EZEB
                    |aircraftop=EasyJet Airline
                    |aircraftact=
                    |imagetype=Photograph
                    |imageloc=Malaga (AGP / LEMG), Spain
                    |imagedate=2004-06-27
                    |imageauthor=Javier Bravo Muñoz
                    |imagesource=
                    *Gallery page http://www.airliners.net/photo/EasyJet-Airline/Airbus-A319-111/0615505/L
                    *Photo http://cdn-www.airliners.net/aviation-photos/photos/5/0/5/0615505.jpg
                    |permission=
                    |other_versions=
                    |other_fields={{Information field|name=Construction number|value=2120}}
                    }}";
        }
        
        [Test]
        public void License_loader_should_get_license_info()
        {
            var licenseInfoLoader = Resolve<WikiImageLicenseLoader>();
            var licenseInfo = licenseInfoLoader.Run("Platichthys_flesus_Vääna-Jõesuu_in_Estonia.jpg", "commons.wikimedia.org");
            Assert.That(licenseInfo.AuthorName, Is.EqualTo("Tiit Hunt"));
        }

        [Test]
        public void Should_parse_markup_of_image_details_page()
        {
            var demoText = GetMarkupDemoText_mld_template();

            var parsedImageMakup = ParseImageMarkup.Run(demoText);
            var infoSectionParams = parsedImageMakup.InfoTemplate.Parameters;

            Assert.That(infoSectionParams.Count, Is.EqualTo(7));
            Assert.That(infoSectionParams[0].Key, Is.EqualTo("Description"));
            Assert.That(infoSectionParams[1].Key, Is.EqualTo("Date"));
            Assert.That(infoSectionParams[1].Value, Is.EqualTo("2010-01-06"));

            Assert.That(parsedImageMakup.Description_Raw,
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
            var demoText = GetMarkupDemoText_several_languages_with_de();

            var parsedImageMakup = ParseImageMarkup.Run(demoText);
            Assert.That(parsedImageMakup.Description_Raw, Is.EqualTo("Blick in den Kolob Canyons. ''Kolob Canyons'' ist der nordwestliche Teil vom [[:de:Zion National Park|Zion National Park]], [[:de:Utah|Utah]], [[:de:Vereinigte Staaten|USA]]. Er gehört zu dem hochgelegenen [[:de:Colorado-Plateau|Colorado-Plateau]] und ist bekannt geworden durch die prächtigen Farben seiner Gebirgslandschaft. Er wird trotz seiner Schönheit selten aufgesucht, weil es in dem Zion National Park keine Straße gibt, die dorthin führt. Die Straße in den ''Kolob Canyons'' beginnt an der [[:de:Interstate_15#Utah|Interstate_15]] etwa 20 Meilen südlich der Stadt [[:de:Cedar City (Utah)|Cedar City]]."));
        }

        [Test]
        public void Should_parse_template_with_parameters_and_subtemplates()
        {
            


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

            Assert.That(LicenseRepository.GetAllRegisteredLicenses().Any(registeredLicense => !String.IsNullOrEmpty(registeredLicense.WikiSearchString) && registeredLicense.WikiSearchString.ToLower() == "cc-by-sa-3.0"), Is.True, "GetAll failed");
            Assert.That(LicenseParser.GetAuthorizedParsedLicenses(markup).Any(parsedLicense => parsedLicense.WikiSearchString.ToLower() == "cc-by-sa-3.0"), Is.True,"GetApplicable failed");
        }

        [Test]
        public void TempFillImageData()
        {   
            //Later RunWikiMedia/StoreWiki should be tested instead
            const string markup = @"== {{int:license-header}} ==
                                    {{self|GFDL|cc-by-sa-3.0,2.5,2.0,1.0|cc-by-sa-3.0}}
                                    {{svg|sport}}";

            var mainLicense = LicenseParser.GetMainLicenseId(markup);
            var allRegisteredLicenses = string.Join(",", LicenseParser.GetAllParsedLicenses(markup).Select(x => x.Id.ToString()));

            Console.WriteLine("Main license id: " + mainLicense);
            Console.WriteLine("Ids: " + allRegisteredLicenses);

        }

