using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs067
    {
        public static void Run()
        {
            RenameFiles(new QuestionImageSettings().ServerPath());
            RenameFiles(new SetImageSettings().ServerPath());
            RenameFiles(new UserImageSettings(-1).ServerPath());
            RenameFiles(new CategoryImageSettings().ServerPath());
        }

        private static void RenameFiles(string basePath)
        {
            var allFilesInDirectory = Directory.GetFiles(basePath, "*.jpg");
            var allFilesWithoutUnderscore = allFilesInDirectory.Where(x => !x.Contains("_"));

            foreach (var imagePath in allFilesWithoutUnderscore)
            {
                int imageWidth;
                using (var image = Image.FromFile(imagePath))
                {
                    imageWidth = image.Width;
                }

                var newFilePath = imagePath.Replace(".jpg", "_" + imageWidth + ".jpg");
                if (File.Exists(newFilePath))
                    File.Delete(imagePath);
                else
                    File.Move(imagePath, newFilePath);
            }
        }
    }
}
