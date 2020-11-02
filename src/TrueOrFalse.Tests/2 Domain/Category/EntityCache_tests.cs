using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TrueOrFalse.Tests;


class EntityCache_tests : BaseTest
    {

        [Test]
        public void Should_direct_childrens()
        {

            var context = ContextCategory.New();

            var parent = context.Add("RootElement").Persist().All.First();

            var firstChildrens = context
                .Add("Sub1", parent: parent)
                .Persist()
                .All;

            var secondChildren = context.
                Add("SubSub1", parent: firstChildrens.ByName("Sub1"))
                .Persist()
                .All
                .ByName("SubSub1");

            var directChildren = EntityCache.GetChildrens(parent.Id).First(); 
        Assert.That(directChildren.Name, Is.EqualTo("Sub1"));
        }
    }

