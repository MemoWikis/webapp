using System;
using System.Linq;
using NUnit.Framework;
using SolrNet;
using TrueOrFalse.Search;

namespace TrueOrFalse.Tests;

public class CategorySearchTests : BaseTest
{
    [Test]
    public void Should_insert_and_query_document()
    {
        var categorySolrMap = new CategorySolrMap
        {
            Id = 1,
            DateCreated = DateTime.Now,
            CreatorId = 1,
            Name = "Name",
            Description = "Foo Bar"
        };

        var solrOperations = Resolve<ISolrOperations<CategorySolrMap>>();
        solrOperations.Delete(new SolrQuery("*:*"));
        solrOperations.Commit();

        solrOperations.Add(categorySolrMap);
        solrOperations.Commit();

        var result1 = solrOperations.Query(new SolrQueryByField("FullTextStemmed", "Foo"));
        Assert.That(result1.Count, Is.EqualTo(1));

        var result2 = solrOperations.Query(new SolrQueryByField("FullTextStemmed", "foo"));
        Assert.That(result2.SpellChecking.First().Suggestions.First(), Is.EqualTo("Foo"));
    }

    [Test]
    public void Should_order_search_result()
    {
        R<ReIndexAllCategories>().Run();

        var context = ContextCategory.New()
            .Add("1").QuestionCount(10)
            .Add("2").QuestionCount(50)
            .Add("3").QuestionCount(1)
            .Persist();

        var categories = R<CategoryRepository>().GetByIds(
            R<SearchCategories>().Run("", orderBy: SearchCategoriesOrderBy.QuestionCount).CategoryIds);
            
        Assert.That(categories.Count, Is.EqualTo(3));
        Assert.That(categories.Count(x => x.Name == "2"), Is.EqualTo(1));
        Assert.That(categories.Count(x => x.Name == "1"), Is.EqualTo(1));
        Assert.That(categories.Count(x => x.Name == "3"), Is.EqualTo(1));
    }
}