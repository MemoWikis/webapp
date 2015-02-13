using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using SolrNet.Utils;
using TrueOrFalse.Tests;
using TrueOrFalse.WikiMarkup;


namespace TrueOrFalse.Tests._2_Domain.Image
{
    internal class ImageInfo_from_Wikimedia : BaseTest
    {
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

        public static string GetMarkupDemoText_author_has_custom_user_template()
        {
            return @"== {{int:filedesc}} ==
                    {{Information
                        |description    = 
                    {{de|Sonnenaufgang im Morgennebel nahe [[:de:Dülmen|Dülmen]], Nordrhein-Westfalen, Deutschland}}
                    {{en|Sunrise in morning mist near [[:en:Dülmen|Dülmen]], North Rhine-Westphalia, Germany}}
                    {{fr|1=Des rayons de soleil matinaux sont visibles entre des arbres bordant une route près de [[:fr:Dülmen|Dülmen]], en Rhénanie du Nord-Westphalie, Allemagne.}}
                        |date           = 2012-09-16 08:08:42
                        |source         = {{own}}
                        |author         = {{User:XRay/Templates/Author}}
                        |permission     = {{User:XRay/Templates/Credits/cc-by-sa}}
                        |other_versions =
                        |other_fields   = {{User:XRay/Templates/Credits/Attribution}}
                    }}
                    {{Location dec|51.821097|7.258872|}}

                    == {{Assessment}} ==
                    {{Assessments|featured=1|quality=1}}
                    {{picture of the day|year=2014|month=11|day=13}}

                    == {{anchor|ImgLic}}{{int:license-header}} ==
                    {{self|author={{User:XRay/Templates/Author}}|cc-by-sa-3.0|cc-by-sa-3.0-de|cc-by-sa-4.0}}

                    {{User:XRay/Templates/Notes}}

                    [[Category:Golden hour (photography)]]
                    [[Category:Nature of Dülmen]]
                    [[Category:Streets in Dülmen]]
                    [[Category:Summer 2012 in Germany]]
                    [[Category:Sunlight through trees]]
                    [[Category:Sunrises of North Rhine-Westphalia]]
                    [[Category:Trees at sunset]]
                    [[Category:2012 in Dülmen]]
                    [[Category:Summer in Dülmen]]
                    [[Category:September 2012 in Germany]]
                    [[Category:Photographs taken on 2012-09-16]]
                    [[Category:Taken with Canon EF-S 18-135mm F3.5-5.6 IS]]
                    [[Category:Taken with Canon EOS 600D]]
                    [[Category:Featured pictures of Dülmen]]
                    [[Category:Featured pictures of landscapes]]
                    [[Category:Quality images of Dülmen]]
                    [[Category:Quality images of landscapes]]
                    [[Category:Photographs by Dietmar Rabich/Dülmen]]
                    [[Category:Photographs by Dietmar Rabich/Featured pictures]]
                    [[Category:Photographs by Dietmar Rabich/Quality images]]

                    ";
        }

        [Test]
        public void License_loader_should_get_image_info()
        {
            var licenseInfoLoader = Resolve<WikiImageLicenseLoader>();
            var licenseInfo = licenseInfoLoader.Run("Platichthys_flesus_Vääna-Jõesuu_in_Estonia.jpg", "commons.wikimedia.org");
            Assert.That(licenseInfo.AuthorName, Is.EqualTo("Tiit Hunt (User: Tiithunt)"));
        }

        [Test]
        public void Should_parse_markup_of_image_details_page()
        {
            var demoText = GetMarkupDemoText_mld_template();

            var parsedImageMarkup = ParseImageMarkup.Run(demoText);
            var infoSectionParams = parsedImageMarkup.InfoTemplate.Parameters;

            Assert.That(infoSectionParams.Count, Is.EqualTo(7));
            Assert.That(infoSectionParams[0].Key, Is.EqualTo("Description"));
            Assert.That(infoSectionParams[1].Key, Is.EqualTo("Date"));
            Assert.That(infoSectionParams[1].Value, Is.EqualTo("2010-01-06"));

            Assert.That(parsedImageMarkup.Description_Raw,
                Is.EqualTo(
                    "Eine [[:de:Flunder|Flunder]], ''Platichthys flesus'', nahe dem estnischen Dorf [[:de:Vääna-Jõesuu|Vääna-Jõesuu]]"));
            Assert.That(parsedImageMarkup.Description,
                Is.EqualTo("Eine Flunder (http://de.wikipedia.org/wiki/Flunder), <i>Platichthys flesus</i>, nahe dem estnischen Dorf Vääna-Jõesuu (http://de.wikipedia.org/wiki/Vääna-Jõesuu)"));

            Assert.That(parsedImageMarkup.AuthorName_Raw, Is.EqualTo("[[User:Tiithunt|Tiit Hunt]]"));
            Assert.That(parsedImageMarkup.AuthorName, Is.EqualTo("Tiit Hunt (User: Tiithunt)"));
        }

        [Test]
        public void Should_parse_de_description()
        {
            var demoText = GetMarkupDemoText_several_languages_with_de();

            var parsedImageMakup = ParseImageMarkup.Run(demoText);
            Assert.That(parsedImageMakup.Description_Raw, Is.EqualTo("Blick in den Kolob Canyons. ''Kolob Canyons'' ist der nordwestliche Teil vom [[:de:Zion National Park|Zion National Park]], [[:de:Utah|Utah]], [[:de:Vereinigte Staaten|USA]]. Er gehört zu dem hochgelegenen [[:de:Colorado-Plateau|Colorado-Plateau]] und ist bekannt geworden durch die prächtigen Farben seiner Gebirgslandschaft. Er wird trotz seiner Schönheit selten aufgesucht, weil es in dem Zion National Park keine Straße gibt, die dorthin führt. Die Straße in den ''Kolob Canyons'' beginnt an der [[:de:Interstate_15#Utah|Interstate_15]] etwa 20 Meilen südlich der Stadt [[:de:Cedar City (Utah)|Cedar City]]."));
        }

        [Test]
        [Ignore]
        public void Should_parse_author_and_description()
        {
            var markup = "";
            var parseImageMarkupResult = ParseImageMarkup.Run(markup);
            Assert.That(parseImageMarkupResult.InfoTemplate == null, Is.True,
                "InfoTemplate missing - template null expected");
            Assert.That(ImageParsingNotifications.FromJson(parseImageMarkupResult.Notifications).InfoTemplate
                .Any(notification => notification.Name == "No information template found"), Is.True,
                "InfoTemplate missing - notification expected");

            markup = @"{{Information
                        |Source={{own}}, Collection [[User:Michael Gäbler|Michael Gäbler]]
                        ";
            parseImageMarkupResult = ParseImageMarkup.Run(markup);
            Assert.That(parseImageMarkupResult.Description == null, Is.True,
                  "Description parameter missing - description null expected");
            Assert.That(ImageParsingNotifications.FromJson(parseImageMarkupResult.Notifications).Description
                  .Any(notification => notification.Name == "No description parameter found"), Is.True,
                  "Description parameter missing - notification expected");
            Assert.That(parseImageMarkupResult.AuthorName == null, Is.True,
                  "Author parameter missing - AuthorName null expected");
            Assert.That(ImageParsingNotifications.FromJson(parseImageMarkupResult.Notifications).Author
                  .Any(notification => notification.Name == "No author parameter found"), Is.True,
                  "Author parameter missing - notification expected");

            markup = @"{{Information
                        |Description=
                        |Author=
                        ";
            parseImageMarkupResult = ParseImageMarkup.Run(markup);
            Assert.That(parseImageMarkupResult.Description == null, Is.True,
                        "Description parameter empty - description null expected");
            Assert.That(ImageParsingNotifications.FromJson(parseImageMarkupResult.Notifications).Description
                        .Any(notification => notification.Name == "Description parameter empty"), Is.True,
                        "Description parameter empty - notification expected");
            Assert.That(parseImageMarkupResult.AuthorName == null, Is.True,
                        "Author parameter empty - author null expected");
            Assert.That(ImageParsingNotifications.FromJson(parseImageMarkupResult.Notifications).Author
                        .Any(notification => notification.Name == "Author parameter empty"), Is.True,
                        "Author parameter empty - notification expected");

            markup = @"{{Information
                        |Description=
                        |Author={{User:XRay/Templates/Author}}
                        ";
            parseImageMarkupResult = ParseImageMarkup.Run(markup);
            Assert.That(parseImageMarkupResult.AuthorName == null, Is.True,
                            "Custom wiki user template - author null expected");
            Assert.That(ImageParsingNotifications.FromJson(parseImageMarkupResult.Notifications).Author
                        .Any(notification => notification.Name == "Custom wiki user template"), Is.True,
                        "Custom wiki user template - notification expected");

            markup = @"{{Information
                        |Description=Plain text
                        |Author=Plain text
                        ";

            parseImageMarkupResult = ParseImageMarkup.Run(markup);
            Assert.That(parseImageMarkupResult.Description == "Plain text", Is.EqualTo("Plain text"),
                           "Description: Take plain text as is");
            Assert.That(parseImageMarkupResult.AuthorName, Is.EqualTo("Plain text"),
                           "Author: Take plain text as is");

            markup = @"{{Information
                        |Description=[[undefined internal wiki link]]
                        |Author=[[undefined internal wiki link]]
                        ";
            parseImageMarkupResult = ParseImageMarkup.Run(markup);
            Assert.That(parseImageMarkupResult.Description == null, Is.True,
                           "Unparsed wiki markup - description null expected");
            Assert.That(ImageParsingNotifications.FromJson(parseImageMarkupResult.Notifications).Description
                        .Any(notification => notification.Name == "Manual entry for description required"), Is.True,
                        "Description: Unparsed wiki markup - notification expected");
            Assert.That(parseImageMarkupResult.AuthorName == null, Is.True,
                            "Unparsed wiki markup - author null expected");
            Assert.That(ImageParsingNotifications.FromJson(parseImageMarkupResult.Notifications).Author
                        .Any(notification => notification.Name == "Manual entry for author required"), Is.True,
                        "Author: Unparsed wiki markup - notification expected");
            new License(){}.InitLicenseSettings();
        }

//        [Test]

    }
}