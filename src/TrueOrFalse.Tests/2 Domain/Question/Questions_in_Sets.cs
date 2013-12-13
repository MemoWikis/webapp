using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TrueOrFalse.Tests.Persistence
{
    public class Questions_in_Sets
    {
        [Test]
        public void Should_serialize_and_deserialize_JSON_set_minis()
        {
            var question = new Question();
            question.SetTop5Minis = new List<SetMini>();
            Assert.That(question.SetTop5Minis.Count, Is.EqualTo(0));

            question.SetTop5Minis = 
                new List<SetMini>{
                    new SetMini{Id = 1, Name = "Name1"},
                    new SetMini{Id = 2, Name = "Name2"}
                };
            Assert.That(question.SetTop5Minis.Count, Is.EqualTo(2));
            Assert.That(question.SetTop5Minis[1].Name, Is.EqualTo("Name2"));
        }
    }
}
