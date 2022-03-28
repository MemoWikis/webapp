using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

[TestFixture]
public class CategoryAuthorIds_tests
{
    [Test]
    public void ShouldAddAuthorId()
    {
        var category = new Category();

        Assert.That(category.AuthorIdsInts.Length, Is.EqualTo(0));

        category.AddAuthor(21);
        Assert.That(category.AuthorIds, Is.EqualTo("21"));
        Assert.That(category.AuthorIdsInts[0], Is.EqualTo(21));


        category.AddAuthor(21);
        Assert.That(category.AuthorIds, Is.EqualTo("21"));

        category.AddAuthor(25);
        Assert.That(category.AuthorIds, Is.EqualTo("21, 25"));
        Assert.That(category.AuthorIdsInts[0], Is.EqualTo(21));
        Assert.That(category.AuthorIdsInts[1], Is.EqualTo(25));
    }
}
