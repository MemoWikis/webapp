using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using NHibernate.Mapping;

namespace TrueOrFalse
{
    public class ImageMaintenanceInfo
    {
        public int ImageId;
        public int TypeId;

        public bool InQuestionFolder;
        public bool InCategoryFolder;
        public bool InSetFolder;

        public ImageMetaData MetaData;

        public string Url_128;

        //new
        public License MainLicense;
        public List<License> AllLicenses;
        public string LicenseStateHtmlList;

        public bool AllRequiredMainLicenseInfosPresent;

        public List<string> PossibleLicenseStrings;


        public ImageMaintenanceInfo(ImageMetaData imageMetaData)
        {
            var categoryImgBasePath = new CategoryImageSettings().BasePath;
            var questionImgBasePath = new QuestionImageSettings().BasePath;
            var setImgBasePath = new QuestionSetImageSettings().BasePath;

            ImageId = imageMetaData.Id;
            MetaData = imageMetaData;
            TypeId = imageMetaData.TypeId;
            //new
            MainLicense = LicenseParser.GetMainLicense(imageMetaData.Markup);
            AllLicenses = LicenseParser.GetAllParsedLicenses(imageMetaData.Markup);
            LicenseStateHtmlList = !String.IsNullOrEmpty(ToLicenseStateHtmlList(AllLicenses, MetaData.Markup)) ?
                //"<ul>" + 
                ToLicenseStateHtmlList(AllLicenses, MetaData.Markup)
               // + "</ul>" 
                : "";

            InCategoryFolder = File.Exists(HttpContext.Current.Server.MapPath(
                categoryImgBasePath + imageMetaData.TypeId + ".jpg"));
            InQuestionFolder = File.Exists(HttpContext.Current.Server.MapPath(
                questionImgBasePath + imageMetaData.TypeId + ".jpg"));
            InSetFolder = File.Exists(HttpContext.Current.Server.MapPath(
                setImgBasePath + imageMetaData.TypeId + ".jpg"));

            if (MetaData.Type == ImageType.Category)
                Url_128 = new CategoryImageSettings(MetaData.TypeId).GetUrl_128px(asSquare: true).Url;
            
            if (MetaData.Type == ImageType.Question)
                Url_128 = new QuestionImageSettings(MetaData.TypeId).GetUrl_128px_square().Url;

            if (MetaData.Type == ImageType.QuestionSet)
                Url_128 = QuestionSetImageSettings.Create(MetaData.TypeId).GetUrl_128px_square().Url;
        }

        private int GetAmountMatches()
        {
            int amountTrues = 0;
            if (InQuestionFolder) amountTrues++;
            if (InCategoryFolder) amountTrues++;
            if (InSetFolder) amountTrues++;
            return amountTrues;
        }

        public bool IsNothing()
        {
            return !InQuestionFolder && !InCategoryFolder && !InSetFolder;
        }

        public bool IsClear()
        {
            return GetAmountMatches() == 1;
        }

        public bool IsNotClear()
        {
            return GetAmountMatches() > 1;
        }

        public string GetCssClass()
        {
            if (IsClear())
                return "success";

            if (IsNothing())
                return "warning";

            if (IsNotClear())
                return "danger";

            return "";
        }

        public ImageType GetImageType()
        {
            if (IsClear())
            {
                if (InQuestionFolder)
                    return ImageType.Question;

                if (InCategoryFolder)
                    return ImageType.Category;

                if (InSetFolder)
                    return ImageType.QuestionSet;
            }

            throw new Exception("no clear type");
        }

        public static string ToLicenseStateHtmlList(List<License> licenseList, string wikiMarkup)
        {
            return licenseList.Count > 0
                ? "<ul>" + 
                    licenseList
                        .Aggregate("",
                            (current, license) =>
                                current + "<li>" +
                                (!String.IsNullOrEmpty(license.LicenseShortName)
                                    ? license.LicenseShortName
                                    : license.WikiSearchString) + " (" +
                                LicenseParser.CheckLicenseState(license, wikiMarkup) + ")</li>")
                    + "</ul>"
                : "";
        }
    }
}