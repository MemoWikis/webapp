using System.Collections.Generic;
using NUnit.Framework;

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
