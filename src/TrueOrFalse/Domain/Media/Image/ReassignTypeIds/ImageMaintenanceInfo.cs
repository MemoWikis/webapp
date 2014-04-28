using System;
using System.Drawing;
using System.IO;
using System.Web;
using FluentNHibernate.Conventions.AcceptanceCriteria;

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

        public ImageMaintenanceInfo(ImageMetaData imageMetaData)
        {
            var categoryImgBasePath = new CategoryImageSettings().BasePath;
            var questionImgBasePath = new QuestionImageSettings().BasePath;
            var setImgBasePath = new QuestionSetImageSettings().BasePath;

            ImageId = imageMetaData.Id;
            MetaData = imageMetaData;
            TypeId = imageMetaData.TypeId;
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
    }
}