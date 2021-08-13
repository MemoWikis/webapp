using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Autofac;
using NHibernate;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class CreateSitemap : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                // Create the file, or overwrite if the file exists.
                //Create sitemap_index
                var pathSiteIndex = PathTo.Sitmap_xml("sitemap_index.xml"); 
                    using (FileStream fs = File.Create(pathSiteIndex))
                    {
                        var content =
                            "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
                            "<sitemapindex xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">\r\n " + 
                                "<sitemap>\r\n    <loc>http://www.example.com/categories.xml</loc>\r\n </sitemap>\r\n" +
                                "<sitemap>\r\n    <loc>http://www.example.com/questions.xml</loc>\r\n  </sitemap>\r\n" +
                            "</sitemapindex>"; 

                        byte[] info = new UTF8Encoding(true).GetBytes(content);
                        // Add some information to the file.
                        fs.Write(info, 0, info.Length);
                    }

                    //Create Categories
                    var pathSitemap = PathTo.Sitmap_xml("categories.xml");
                    using (FileStream fs = File.Create(pathSitemap))
                    {
                        var categories =   Sl.CategoryRepo.GetAll().Take(30);
                        var content = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
                             "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">\r\n";
                    foreach (var category in categories)
                        {
                            content += 
                                "<url>\r\n    " +
                                    "<loc>http://www.memucho.de/"+ category.Name + "/"+ category.Id + "</loc>\r\n" +
                                "</url>\r\n";
                        }
                        content += "</urlset>";
                        byte[] info = new UTF8Encoding(true).GetBytes(content);
                        fs.Write(info, 0, info.Length);
                    }
                Logg.r().Information("CreateSitemap last start ", DateTime.Now);
            }, "CreateSitemap");
        }
    }
}
