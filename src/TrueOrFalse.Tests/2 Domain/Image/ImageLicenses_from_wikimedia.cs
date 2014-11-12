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
    internal class ImageLicenses_from_wikimedia : BaseTest
    {
        [Test]
        public void Should_sort_licenses()
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

        public void Should_find_main_license()
        {

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
        public void Authorized_licenses_should_contain_all_necessary_information()
        {
            var allAuthorizedLicenses = LicenseRepository.GetAllAuthorizedLicenses();

            //Check for licenses without valid id
            Assert.That(allAuthorizedLicenses.All(license => license.Id > 0),
                        CreateErrorMessage(allAuthorizedLicenses.Where(license => !(license.Id > 0)),
                                            "No valid id assigned for:"));

            //Check for licenses without search string
            Assert.That(allAuthorizedLicenses.All(license => !String.IsNullOrEmpty(license.WikiSearchString)),
                        CreateErrorMessage(allAuthorizedLicenses.Where(license => String.IsNullOrEmpty(license.WikiSearchString)),
                                            "No search string found for:"));

            //Check for licenses without information about author requirement
            Assert.That(allAuthorizedLicenses.All(license => license.AuthorRequired.HasValue),
                        CreateErrorMessage(allAuthorizedLicenses.Where(license => !license.AuthorRequired.HasValue),
                                            "No information about author requirement provided for:"));

            //Check for licenses without sufficient license link information
            Assert.That(allAuthorizedLicenses.All(license => license.LicenseLinkRequired.HasValue),
                        CreateErrorMessage(allAuthorizedLicenses.Where(license => !license.LicenseLinkRequired.HasValue),
                                            "No information about license link requirement provided for:"));

            Assert.That(allAuthorizedLicenses.Where(license => license.LicenseLinkRequired.IsTrue()).All(license => !String.IsNullOrEmpty(license.LicenseLink)),
                        CreateErrorMessage(allAuthorizedLicenses.Where(license => license.LicenseLinkRequired.IsTrue() && String.IsNullOrEmpty(license.LicenseLink)),
                                            "Necessary license link not provided for:"));

            Assert.That(allAuthorizedLicenses.Where(license => license.CopyOfLicenseTextRequired.IsTrue()).All(license => !String.IsNullOrEmpty(license.CopyOfLicenseTextUrl)),
                        CreateErrorMessage(allAuthorizedLicenses.Where(license => license.CopyOfLicenseTextRequired.IsTrue() && String.IsNullOrEmpty(license.CopyOfLicenseTextUrl)),
                                            "Necessary url of local license copy not provided for:"));
        }

        public static string CreateErrorMessage(IEnumerable<License> erroneousLicenses, string messageSeed)
        {
            return erroneousLicenses.Aggregate(messageSeed + Environment.NewLine,
                        (current, license) => current + String.Format("Id: {0}, SearchString: {1}" + Environment.NewLine,
                            license.Id, license.WikiSearchString));
        }

        [Test]
        public void Registered_licenses_should_not_contain_duplicates()
        {
            var duplicateIdLicenses =
                LicenseRepository.GetAllRegisteredLicenses()
                    .GroupBy(l => l.Id)
                    .Where(grp => grp.Count() > 1)
                    .SelectMany(grp => grp)
                    .ToList();

            var duplicateSearchStringLicenses =
                LicenseRepository.GetAllRegisteredLicenses()
                    .GroupBy(l => l.WikiSearchString)
                    .Where(grp => grp.Count() > 1)
                    .SelectMany(grp => grp)
                    .ToList();

            var messageDuplicateSearchStrings = duplicateSearchStringLicenses
                                                .Aggregate("Duplicate search strings: " + Environment.NewLine,
                                                            (current, duplicateSearchStringLicense) => current + String.Format("{0} (Id: {1})" + Environment.NewLine,
                                                                duplicateSearchStringLicense.WikiSearchString, duplicateSearchStringLicense.Id));

            Assert.That(
                !LicenseRepository.GetAllRegisteredLicenses().GroupBy(l => l.Id).Any(grp => grp.Count() > 1),
                "Duplicate ID(s):" + Environment.NewLine + String.Join(Environment.NewLine, duplicateIdLicenses.Select(license => license.Id).ToList()));

            Assert.That(
                !LicenseRepository.GetAllRegisteredLicenses().GroupBy(l => l.WikiSearchString).Any(grp => grp.Count() > 1),
                messageDuplicateSearchStrings);
        }
    }
}
