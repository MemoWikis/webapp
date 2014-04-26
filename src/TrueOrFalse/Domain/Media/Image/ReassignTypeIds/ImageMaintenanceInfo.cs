using System;
using System.Drawing;
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