using System.Linq;
using NUnit.Framework;

namespace TrueOrFalse.Tests.Persistence
{
    [Category(TestCategories.Programmer)]
    public class Set_persistence : BaseTest
    {
        [Test]
        public void Set_should_be_persisted()
        {
            ContextSet.GetPersistedSampleSet();

            RecycleContainer();

            var setFromDB = R<SetRepo>().GetAll()[0];
            Assert.That(setFromDB.QuestionsInSet.Count, Is.EqualTo(4));
        }

        [Test]
        public void Set_should_persist_timecodes()
        {
            var set = ContextSet.GetPersistedSampleSet();
            set.QuestionsInSet.First().Timecode = 10;
            set.QuestionsInSet.Skip(1).First().Timecode = 20;
            set.QuestionsInSet.Skip(2).First().Timecode = 35;

            R<SetRepo>().Update(set);

            RecycleContainer();

            var setFromDB = R<SetRepo>().GetAll()[0];
            Assert.That(setFromDB.QuestionsInSet.First().Timecode, Is.EqualTo(10));
            Assert.That(setFromDB.QuestionsInSet.Skip(1).First().Timecode, Is.EqualTo(20));
            Assert.That(setFromDB.QuestionsInSet.Skip(2).First().Timecode, Is.EqualTo(35));
        }
    }
}