using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using EasyNetQ.Events;
using FluentNHibernate.Utils;
using NHibernate.Criterion;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Mapping;
using SolrNet.Utils;
using TrueOrFalse;
using TrueOrFalse.WikiMarkup;

public class LicenseRepository
{
    public static List<License> GetAllRegisteredLicenses()
    {
        var registeredLicenses = new List<License>()
        { 
            //Don't change IDs!
            //Only set "LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded" if all necessary information is provided
            //(Id, WikiSearchString, LicenseRequirementsType or AuthorRequired/LicenseLinkRequired/CopyOfLicenseTextRequired, LicenseLink/CopyOfLicenseTextUrl if required)
            //Run Tests "Authorized_licenses_should_contain_all_necessary_information()" and "Registered_licenses_should_not_contain_duplicates()"
            //Find template on Wikimedia "http://commons.wikimedia.org/wiki/template:" + WikiSearchString + "?uselang=de"
            //Overview of all wikimedia licenses: https://commons.wikimedia.org/wiki/Commons:Image_copyright_tags_visual
            
            new License()
            {
                Id = 1,
                WikiSearchString = "cc-by-1.0",
                LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                
                LicenseRequirementsType = LicenseRequirementsType.Cc_By,
                LicenseLink = "http://creativecommons.org/licenses/by/1.0/legalcode",
                
                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by/1.0/deed.de",
                LicenseLongName = "Creative Commons: Namensnennung 1.0 Generic",
                LicenseShortName = "CC BY 1.0",
            },

            new License()
            {
                Id = 2,
                WikiSearchString = "cc-by-2.0",
                LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                
                LicenseRequirementsType = LicenseRequirementsType.Cc_By,
                LicenseLink = "http://creativecommons.org/licenses/by/2.0/legalcode",
                
                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by/2.0/deed.de",
                LicenseLongName = "Creative Commons: Namensnennung 2.0 Generic",
                LicenseShortName = "CC BY 2.0",
            },

            new License()
            {
                Id = 3,
                WikiSearchString = "cc-by-2.5",
                LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                
                LicenseRequirementsType = LicenseRequirementsType.Cc_By,
                LicenseLink = "http://creativecommons.org/licenses/by/2.5/legalcode",
                
                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by/2.5/deed.de",
                LicenseLongName = "Creative Commons: Namensnennung 2.5 Generic",
                LicenseShortName = "CC BY 2.5",
            },

            new License()
            {
                Id = 4,
                WikiSearchString = "cc-by-3.0",
                LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                
                LicenseRequirementsType = LicenseRequirementsType.Cc_By,
                LicenseLink = "http://creativecommons.org/licenses/by/3.0/legalcode",
                
                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by/3.0/deed.de",
                LicenseLongName = "Creative Commons: Namensnennung 3.0 Unported",
                LicenseShortName = "CC BY 3.0",
            },

            new License()
            {
                Id = 5,
                WikiSearchString = "cc-by-3.0,2.5,2.0,1.0",
                LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                
                LicenseRequirementsType = LicenseRequirementsType.Cc_By,
                LicenseLink = "http://creativecommons.org/licenses/by/3.0/legalcode",
                
                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by/3.0/deed.de",
                LicenseLongName = "Creative Commons: Namensnennung 3.0 Unported",
                LicenseShortName = "CC BY 3.0",
            },

            new License()
            {
                Id = 6,
                WikiSearchString = "cc-sa-1.0",
                LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                
                LicenseRequirementsType = LicenseRequirementsType.Cc_Sa,
                LicenseLink = "http://creativecommons.org/licenses/sa/1.0/legalcode",
                
                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/sa/1.0/deed.de",
                LicenseLongName = "Creative Commons: Weitergabe unter gleichen Bedingungen 1.0 Generic",
                LicenseShortName = "CC SA 1.0",
            },

            new License()
            {
                Id = 7,
                WikiSearchString = "cc-by-sa-1.0",
                LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                
                LicenseRequirementsType = LicenseRequirementsType.Cc_By_Sa,
                LicenseLink = "http://creativecommons.org/licenses/by-sa/1.0/legalcode",
                
                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by-sa/1.0/deed.de",
                LicenseLongName = "Creative Commons: Namensnennung - Weitergabe unter gleichen Bedingungen 1.0 Generic",
                LicenseShortName = "CC BY-SA 1.0",
            },

            new License()
            {
                Id = 8,
                WikiSearchString = "cc-by-sa-2.0",
                LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                
                LicenseRequirementsType = LicenseRequirementsType.Cc_By_Sa,
                LicenseLink = "http://creativecommons.org/licenses/by-sa/2.0/legalcode",
                
                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by-sa/2.0/deed.de",
                LicenseLongName = "Creative Commons: Namensnennung - Weitergabe unter gleichen Bedingungen 2.0 Generic",
                LicenseShortName = "CC BY-SA 2.0",
            },

            new License()
            {
                Id = 9,
                WikiSearchString = "cc-by-sa-2.5",
                LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                
                LicenseRequirementsType = LicenseRequirementsType.Cc_By_Sa,
                LicenseLink = "http://creativecommons.org/licenses/by-sa/2.5/legalcode",
                
                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by-sa/2.5/deed.de",
                LicenseLongName = "Creative Commons: Namensnennung - Weitergabe unter gleichen Bedingungen 2.5 Generic",
                LicenseShortName = "CC BY-SA 2.5",
            },

            new License()
            {
                Id = 10,
                WikiSearchString = "cc-by-sa-3.0",
                LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,

                LicenseRequirementsType = LicenseRequirementsType.Cc_By_Sa,
                LicenseLink = "http://creativecommons.org/licenses/by-sa/3.0/legalcode",
                
                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by-sa/3.0/deed.de",
                LicenseLongName = "Namensnennung - Weitergabe unter gleichen Bedingungen 3.0 Unported",
                LicenseShortName = "CC BY-SA 3.0",
            },
            
            new License()
            {
                Id = 11,
                WikiSearchString = "cc-by-sa-3.0,2.5,2.0,1.0",
                LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                
                LicenseRequirementsType = LicenseRequirementsType.Cc_By_Sa,
                LicenseLink = "http://creativecommons.org/licenses/by-sa/3.0/legalcode",
                
                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by-sa/3.0/deed.de",
                LicenseLongName = "Creative Commons: Namensnennung - Weitergabe unter gleichen Bedingungen 3.0 Unported",
                LicenseShortName = "CC BY-SA 3.0",
            },

            new License()
            {
                Id = 12,
                WikiSearchString = "cc-by-sa-4.0",
                LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,
                
                LicenseRequirementsType = LicenseRequirementsType.Cc_By_Sa,
                LicenseLink = "http://creativecommons.org/licenses/by-sa/4.0/legalcode",
                
                LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by-sa/4.0/deed.de",
                LicenseLongName = "Creative Commons: Namensnennung - Weitergabe unter gleichen Bedingungen 4.0 International",
                LicenseShortName = "CC BY-SA 4.0",
            },

            //new License()
            //{
            //    Id = 100,
            //    WikiSearchString = "gfdl",
            //    LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,

            //    LicenseRequirementsType = LicenseRequirementsType.GFDL,

            //},

            new License()
            {
                Id = 200,
                WikiSearchString = "pd-old",
                LicenseApplicability = LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded,

                LicenseRequirementsType = LicenseRequirementsType.PD
            }



            //Template for CC-BY-SA licenses:
            //new License()
            //{
            //    Id = 2,
            //    WikiSearchString = "cc-by-sa-3.0,2.5,2.0,1.0",
            //    LicenseApplicability = LicenseApplicability.,//Requirements should be recorded under License > InitLicenseSettings()
                
            //    LicenseRequirementsType = LicenseRequirementsType.Cc_By_Sa,
            //    LicenseLink = "http://creativecommons.org/licenses/by-sa/3.0/legalcode",
                
            //    LicenseShortDescriptionLink = "http://creativecommons.org/licenses/by-sa/3.0/deed.de",
            //    LicenseLongName = "Creative Commons: Namensnennung - Weitergabe unter gleichen Bedingungen 3.0 Unported",
            //    LicenseShortName = "CC BY-SA 3.0",
            //},

            //Template general:
            //new License()
            //{
            //    Id = ,
            //    WikiSearchString = "",
             
            //    Choose RequirementsType or add requirements manually
            //    LicenseRequirementsType = LicenseRequirementsType.,
            //        //LicenseLink = ,
            //        //CopyOfLicenseTextUrl = ,
            //    //or:
            //        AuthorRequired = ,
            //        LicenseLinkRequired = ,
            //        //LicenseLink = ,
            //        CopyOfLicenseTextRequired = ,
            //        //CopyOfLicenseTextUrl = ,

            //    LicenseApplicability = ,
            //}
        };

        registeredLicenses.ForEach(license => license.InitLicenseSettings());

        return registeredLicenses;
    }

    public static List<License> GetAllAuthorizedLicenses()
    {
        return GetAllRegisteredLicenses().Where(license => license.LicenseApplicability == LicenseApplicability.LicenseAuthorizedAndAllRequirementsRecorded).ToList();
        //$temp: What about "LicenseApplicability.LicenseIsConditionallyApplicable"?
    }

    public static License GetById(int id)
    {
        return GetAllRegisteredLicenses().FirstOrDefault(license => license.Id == id);
    }
}

