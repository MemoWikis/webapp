using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;
using NUnit.Framework;
using TrueOrFalse.Tests;

class ModifyRelationsForCategory_tests : BaseTest
{
    [Test]
    public void UpdateCategoryRelationsOfType()
    {
        ContextCategory.New().AddCaseThreeToCache();
        EntityCache.Init();
        ModifyRelationsForCategory.UpdateCategoryRelationsOfType(
            EntityCache.GetCategoryByName("X3").FirstOrDefault().Id, 
            EntityCache.GetCategoryByName("B").GetIds().ToList(), 
            CategoryRelationType.IsChildOf);

        var X3 = EntityCache.GetCategoryByName("X3").First();
        Assert.That(EntityCache.GetCategoryCacheItem(X3.CategoryRelations.First().CategoryId).Name, Is.EqualTo("X3"));
        Assert.That(EntityCache.GetCategoryCacheItem(X3.CategoryRelations.First().RelatedCategoryId).Name, Is.EqualTo("B"));
        Assert.That(ContextCategory.HasCorrectChild(X3, "A"), Is.EqualTo(false));
        
        Assert.That(X3.CategoryRelations.Count,Is.EqualTo(1));

        var B = EntityCache.GetCategoryByName("B").First(); 

    }
}
