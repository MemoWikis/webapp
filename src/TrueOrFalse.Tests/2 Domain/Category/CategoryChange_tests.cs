using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using NUnit.Framework;
using TrueOrFalse.Tests;

[TestFixture]
class CategoryChange_tests : BaseTest
{
    [Test]
    public void Should_save_category_changes()
    {
        var category = ContextCategory.New().Add("Category 1").Persist().All[0];
        category.Name = "Category 2";

        var categoryRepo = LifetimeScope.Resolve<CategoryRepository>(); 
        categoryRepo.Update(category);

        Assert.That(categoryRepo.GetAllEager().ToList().First().Name, Is.EqualTo("Category 2"));
    }

    [Test]
    public void XmlTest()
    {
        var brokenString =
            "&amp;";

        var formatted = CategoryHistoryDetailModel.FormatHtmlString(brokenString);

        var empty = CategoryHistoryDetailModel.FormatHtmlString(null);

        Assert.That(formatted, Is.EqualTo("&"));
        Assert.That(empty, Is.EqualTo(""));
    }

    [Test]
    public void SafeImgXmlTest()
    {
        var imgString =
            "<img src=\"data:image/png;base64,YII=\" alt=\"0\">";

        var formatted = CategoryHistoryDetailModel.FormatHtmlString(imgString);

        Assert.That(formatted, Is.EqualTo("\r\n  <img src=\"data:image/png;base64,YII=\" alt=\"0\">\r\n "));
    }
}