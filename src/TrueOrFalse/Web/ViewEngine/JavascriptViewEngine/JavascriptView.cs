using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace TrueOrFalse.Web
{
    public class JavaScriptView : IView
    {
        private readonly string _fileName;

        public JavaScriptView(string fileName)
        {
            _fileName = fileName;
        }

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            var file = File.ReadAllText(viewContext.HttpContext.Server.MapPath(_fileName));
            writer.Write(file);
        }
    }
}
