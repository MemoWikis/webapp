using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
//using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDDish.Model;
using NUnit.Framework;
using TrueOrFalse.Tests;


class User_world_tests : BaseTest
{
    [Test]
    public void Should_return_correct_categories()
    {
        ContextCategory.New().AddCaseThreeToCache();
        var user = Sl.SessionUser.User;
       
       UserEntityCache.Init();
        var userWorldCacheCategories = UserEntityCache.GetCategories(user.Id).Values.ToList();
        var entityCacheCategories = EntityCache.GetAllCategories().ToList();

        Assert.That(entityCacheCategories.ByName("I").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "C").Count, Is.EqualTo(1));

        //userWorldCacheCategories
        //Assert.That(typeof(IDictionary).IsAssignableFrom( userWorldCacheCategories), Is.EqualTo(true));
        //var categoriesList = new List<Category>();
        //var t = userWorldCacheCategories; 

    }
}

