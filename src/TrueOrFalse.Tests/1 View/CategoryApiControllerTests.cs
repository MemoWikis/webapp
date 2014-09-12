using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping;
using NUnit.Framework;
using TrueOrFalse.View.Web.Views.Api;

namespace TrueOrFalse.Tests
{
    public class CategoryApiControllerTests
    {
        [Test]
        public void TermExistAsCategory()
        {
            //Check that that no exceptions are thrown
            CategoryApiController.TermExistsAsCategory(null, new List<CategoryJsonResult>());
            CategoryApiController.TermExistsAsCategory("", new List<CategoryJsonResult>());
            CategoryApiController.TermExistsAsCategory(null, new List<CategoryJsonResult>
            {
                new CategoryJsonResult(),
                new CategoryJsonResult()
            }); //empty category names



        }
    }
}
