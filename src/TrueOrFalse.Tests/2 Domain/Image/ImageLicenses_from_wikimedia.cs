using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SolrNet.Utils;
using static System.String;

internal class ImageLicenses_from_wikimedia : BaseTest
{
    [Test]
    public void Authorized_licenses_should_contain_all_necessary_information()
    {
        var allAuthorizedLicenses = LicenseImageRepo.GetAllAuthorizedLicenses().Where(l => l.Id != 555).ToList();

        //Check for licenses without valid id
        Assert.That(allAuthorizedLicenses.All(license => license.Id > 0),
                    CreateErrorMessage(allAuthorizedLicenses.Where(license => !(license.Id > 0)),
                                        "No valid id assigned for:"));

        //Check for licenses without search string
        Assert.That(allAuthorizedLicenses.All(license => !IsNullOrEmpty(license.WikiSearchString)),
                    CreateErrorMessage(allAuthorizedLicenses.Where(license => IsNullOrEmpty(license.WikiSearchString)),
                                        "No search string found for:"));

        //Check for licenses without information about author requirement
        Assert.That(allAuthorizedLicenses.All(license => license.AuthorRequired.HasValue),
                    CreateErrorMessage(allAuthorizedLicenses.Where(license => !license.AuthorRequired.HasValue),
                                        "No information about author requirement provided for:"));

        //Check for licenses without sufficient license link information
        Assert.That(allAuthorizedLicenses.All(license => license.LicenseLinkRequired.HasValue),
                    CreateErrorMessage(allAuthorizedLicenses.Where(license => !license.LicenseLinkRequired.HasValue),
                                        "No information about license link requirement provided for:"));

        Assert.That(allAuthorizedLicenses.Where(license => license.LicenseLinkRequired.IsTrue()).All(license => !IsNullOrEmpty(license.LicenseLink)),
                    CreateErrorMessage(allAuthorizedLicenses.Where(license => license.LicenseLinkRequired.IsTrue() && IsNullOrEmpty(license.LicenseLink)),
                                        "Necessary license link not provided for:"));

        Assert.That(allAuthorizedLicenses.Where(license => license.CopyOfLicenseTextRequired.IsTrue()).All(license => !IsNullOrEmpty(license.CopyOfLicenseTextUrl)),
                    CreateErrorMessage(allAuthorizedLicenses.Where(license => license.CopyOfLicenseTextRequired.IsTrue() && IsNullOrEmpty(license.CopyOfLicenseTextUrl)),
                                        "Necessary url of local license copy not provided for:"));
    }

    public static string CreateErrorMessage(IEnumerable<LicenseImage> erroneousLicenses, string messageSeed)
    {
        return erroneousLicenses.Aggregate(messageSeed + Environment.NewLine,
                    (current, license) => current + Format("Id: {0}, SearchString: {1}" + Environment.NewLine,
                        license.Id, license.WikiSearchString));
    }


    [Test]
    public void Registered_licenses_should_not_contain_duplicates()
    {
        var duplicateIdLicenses =
            LicenseImageRepo.GetAllRegisteredLicenses()
                .GroupBy(l => l.Id)
                .Where(grp => grp.Count() > 1)
                .SelectMany(grp => grp)
                .ToList();

        var duplicateSearchStringLicenses =
            LicenseImageRepo.GetAllRegisteredLicenses()
                .GroupBy(l => l.WikiSearchString)
                .Where(grp => grp.Count() > 1)
                .SelectMany(grp => grp)
                .ToList();

        var messageDuplicateSearchStrings = duplicateSearchStringLicenses
                                            .Aggregate("Duplicate search strings: " + Environment.NewLine,
                                                        (current, duplicateSearchStringLicense) => current + Format("{0} (Id: {1})" + Environment.NewLine,
                                                            duplicateSearchStringLicense.WikiSearchString, duplicateSearchStringLicense.Id));

        Assert.That(
            !LicenseImageRepo.GetAllRegisteredLicenses().GroupBy(l => l.Id).Any(grp => grp.Count() > 1),
            "Duplicate ID(s):" + Environment.NewLine + Join(Environment.NewLine, duplicateIdLicenses.Select(license => license.Id).ToList()));

        Assert.That(
            !LicenseImageRepo.GetAllRegisteredLicenses().GroupBy(l => l.WikiSearchString).Any(grp => grp.Count() > 1),
            messageDuplicateSearchStrings);
    }
        
    [Test]
    public void Should_sort_licenses()
    {

        var sortedLicenses = new List<LicenseImage>()
        {
            new LicenseImage {WikiSearchString = "other"},
            new LicenseImage {WikiSearchString = "gfdl", LicenseRequirementsType = LicenseRequirementsType.GFDL},
            new LicenseImage {WikiSearchString = "cc-by-sa-3.0", LicenseRequirementsType = LicenseRequirementsType.Cc_By_Sa},
            new LicenseImage {WikiSearchString = "pd", LicenseRequirementsType = LicenseRequirementsType.PD},
            new LicenseImage {WikiSearchString = "cc-by-sa-3.0-fr", LicenseRequirementsType = LicenseRequirementsType.Cc_By_Sa},
            new LicenseImage {WikiSearchString = "cc-sa-3.0", LicenseRequirementsType = LicenseRequirementsType.Cc_Sa},
            new LicenseImage {WikiSearchString = "cc-by-sa-2.5", LicenseRequirementsType = LicenseRequirementsType.Cc_By_Sa},
            new LicenseImage {WikiSearchString = "cc-by-sa-3.0-de", LicenseRequirementsType = LicenseRequirementsType.Cc_By_Sa},
            new LicenseImage {WikiSearchString = "alternative"},
        };

        sortedLicenses = LicenseParser.SortLicenses(sortedLicenses);

        var rankList =
            sortedLicenses.Select(
                l => new
                {
                    LReqType = l.LicenseRequirementsType.GetRank(),
                    CcVersion = new GetCcLicenseComponents(l).CcVersion,
                    PrioByCcJurisdictionToken = LicenseParser.PriotizeByCcJurisdictionToken(l),
                    WikiSearchstring = l.WikiSearchString,
                }).ToList();
            
        var sortedLicenseStrings = sortedLicenses.Select(l => l.WikiSearchString).Aggregate((a, b) => a + ", " + b);
        const string expectedResult = "cc-by-sa-3.0, cc-by-sa-3.0-de, cc-by-sa-3.0-fr, cc-by-sa-2.5, cc-sa-3.0, pd, gfdl, alternative, other";

        Assert.That(sortedLicenseStrings.Equals(expectedResult),
                    Format("expected: {0}" + Environment.NewLine + "was: {1}", expectedResult, sortedLicenseStrings));
    }

    [Test]
    public void Should_find_main_license() //$todo: rewrite, store image in testDb, use SuggestMainLicenseFromParsedList() instead of
                                            //SuggestMainLicenseFromMarkup(), remove SuggestMainLicenseFromMarkup()
    {
        var markup = @"
                        {{Information
                        |Author         = [[User:Tiithunt|Tiit Hunt]]
                        }}
                        =={{int:license-header}}==
                        {{self|cc-by-sa-3.0|pd-old|gfdl}}";

        ShowAllLicensesWithNotifications(markup);
        Assert.That(LicenseParser.SuggestMainLicenseFromMarkup(new ImageMetaData{Markup = markup}).WikiSearchString, Is.EqualTo("cc-by-sa-3.0"));

        markup = @"
                    {{Information
                    }}
                    =={{int:license-header}}==
                    {{self|cc-by-sa-3.0|pd-old|gfdl}}";

        ShowAllLicensesWithNotifications(markup);
        Assert.That(LicenseParser.SuggestMainLicenseFromMarkup(new ImageMetaData { Markup = markup }).WikiSearchString, Is.EqualTo("pd-old"));

        markup = @"
                    {{Information
                    }}
                    =={{int:license-header}}==
                    {{self|cc-by-sa-3.0|gfdl}}";

        ShowAllLicensesWithNotifications(markup);
        Assert.That(LicenseParser.SuggestMainLicenseFromMarkup(new ImageMetaData { Markup = markup }), Is.Null);
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

        Assert.That(
            LicenseImageRepo.GetAllRegisteredLicenses()
                .Any(
                    registeredLicense => !IsNullOrEmpty(registeredLicense.WikiSearchString) && 
                    registeredLicense.WikiSearchString.ToLower() == "cc-by-sa-3.0"), 
            Is.True, "GetAll failed"
        );

        var foo = LicenseParser.GetAuthorizedParsedLicenses(markup);

        Assert.That(
            LicenseParser.GetAuthorizedParsedLicenses(markup).Any(parsedLicense => parsedLicense.WikiSearchString.ToLower() == "cc-by-sa-3.0"), 
            Is.True, "GetAuthorizedParsedLicenses failed"
        );
    }

    [Test]
    public void Should_find_other_possible_license_strings()
    {
        //$todo: refine together with tested method
        const string markup = "cc-by-sa-3.0|cc-by-sa-9.0|pd-phantasie|pd|lkjlkjlj";

        var otherPossibleLicenseStrings = LicenseParser.GetOtherPossibleLicenseStrings(markup).Aggregate((a, b) => a + ", " + b);
        const string expectedResult = "cc-by-sa-9.0, pd-phantasie";

        Assert.That(otherPossibleLicenseStrings.Equals(expectedResult),
                    Format("expected: {0}" + Environment.NewLine + "was: {1}", expectedResult, otherPossibleLicenseStrings));
    }

    [Test]
    public void Should_find_cc_by_sa_de()
    {
        const string markup = "{{Cc-by-sa-3.0-de|attribution=Bundesarchiv, B 145 Bild-F078072-0004 / Katherine Young / CC-BY-SA 3.0}}";
        var otherPossibleLicenseStrings = LicenseParser.GetOtherPossibleLicenseStrings(markup).Aggregate((a, b) => a + ", " + b);
        Console.WriteLine(LicenseParser.GetAllParsedLicenses(markup).Select(x => x.LicenseShortName).Aggregate((a, b) => a + ", " + b));
        Assert.That(otherPossibleLicenseStrings, Is.EqualTo("Cc-by-sa-3.0-de"));
    }

    public void ShowAllLicensesWithNotifications(string markup)
    {
        var mainLicense = LicenseParser.SuggestMainLicenseFromMarkup(new ImageMetaData{Markup = markup}) != null ?
                            LicenseParser.SuggestMainLicenseFromMarkup(new ImageMetaData{Markup = markup}).WikiSearchString :
                            "none";
        Console.WriteLine("Main license: " + mainLicense);
            
        var allRegisteredLicenses = Join(", ", LicenseParser.GetAllParsedLicenses(markup).Select(x => x.WikiSearchString.ToString()));
        Console.WriteLine("All registered parsed licenses: " + Environment.NewLine + allRegisteredLicenses);
            
        Console.WriteLine(Environment.NewLine + "All authorized parsed licenses: ");
        LicenseParser.GetAuthorizedParsedLicenses(markup).ForEach(x => Console.WriteLine(Environment.NewLine + x.WikiSearchString + ":"
            + Environment.NewLine + "AllRequirementsMet:" + LicenseParser.CheckLicenseRequirementsWithMarkup(x, new ImageMetaData { Markup = markup }).AllRequirementsMet //$temp: Change to method CheckLicenseRequirementsFromDb() and then remove CheckLicenseRequirementsWithMarkup()
            + Environment.NewLine + "AuthorIsMissing:" + LicenseParser.CheckLicenseRequirementsWithMarkup(x, new ImageMetaData{Markup = markup}).AuthorIsMissing
            + Environment.NewLine + "LicenseLinkIsMissing:" + LicenseParser.CheckLicenseRequirementsWithMarkup(x, new ImageMetaData{Markup = markup}).LicenseLinkIsMissing
            + Environment.NewLine + "LocalCopyOfLicenseUrlMissing:" + LicenseParser.CheckLicenseRequirementsWithMarkup(x, new ImageMetaData{Markup = markup}).LocalCopyOfLicenseUrlMissing
            ));
        Console.WriteLine();
        Console.WriteLine();
    }
}