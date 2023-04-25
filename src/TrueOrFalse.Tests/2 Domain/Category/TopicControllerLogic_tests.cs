
using System.Reflection;
using NUnit.Framework;
using VueApp;
using Xunit;


namespace TrueOrFalse.Tests; 
    public class TopicControllerLogic_tests : BaseTest
    {
       

        [Test]
        public void SaveTopic()
        {
            var context = ContextCategory.New();
            var firstChildren = context
            .Add("private1")
            .Add("private2")
            .Add("private3")
            .Persist()
            .All;

            FieldInfo field = typeof(PermissionCheck).GetField("_privateTopics", BindingFlags.NonPublic | BindingFlags.Static);
            field.SetValue(null, 2);


            var logik = new TopicControllerLogic();
            //var ergebnis = logik.SaveTopic(); 
            

        var l =  EntityCache.GetAllCategories(); 
        }
    }

