﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gibraltar.Agent.Windows;

namespace TrueOrFalse.Maintenance
{
    public class LoadImageMarkups : IRegisterAsInstancePerLifetime
    {
        private readonly ImageMetaDataRepository _imgRepo;
        private readonly WikiImageLicenseLoader _wikiImageLicenseLoader;

        public LoadImageMarkups(
            ImageMetaDataRepository imgRepo,
            WikiImageLicenseLoader wikiImageLicenseLoader)
        {
            _imgRepo = imgRepo;
            _wikiImageLicenseLoader = wikiImageLicenseLoader;
        }


        public void Run(ImageMetaData imageMetaData)
        {
            if(imageMetaData.Source != ImageSource.WikiMedia) return;

            var fileName = imageMetaData.SourceUrl.Split('/').Last();
            var licenseInfo = _wikiImageLicenseLoader.Run(fileName, imageMetaData.ApiHost);

            imageMetaData.AuthorParsed = licenseInfo.AuthorName;
            imageMetaData.DescriptionParsed = licenseInfo.Description;
            imageMetaData.AllRegisteredLicenses = licenseInfo.AllRegisteredLicenses;
            imageMetaData.Markup = licenseInfo.Markup;
            imageMetaData.MarkupDownloadDate = licenseInfo.MarkupDownloadDate;
        }

        //$temp: wird dieses globale (Nach-)Laden des Markups überhaupt benötigt, wenn Daten gleich beim ersten Speichern mit abgerufen werden?
        public void UpdateAll()
        {
            var allImages = _imgRepo.Session
                .QueryOver<ImageMetaData>()
                .Where(x => x.Source == ImageSource.WikiMedia)
                .List<ImageMetaData>();
            
            //Update(allImages);
        }

        public void UpdateAllWithoutAuthorizedMainLicense()
        {
            var allImages = _imgRepo.Session
                .QueryOver<ImageMetaData>()
                .Where(x => x.Source == ImageSource.WikiMedia)
                .List<ImageMetaData>();

            var imagesToUpdate = allImages.Where(x => x.MainLicenseInfo == null ||
                x.ManualEntriesFromJson().ManualImageEvaluation == ManualImageEvaluation.ImageNotEvaluated);

            Update(imagesToUpdate);
        }

        private void Update(IEnumerable<ImageMetaData> imageList)
        {
            foreach (var imageMetaData in imageList)
            {
                Run(imageMetaData);

                _imgRepo.Update(imageMetaData);
            }
        }
    }
}