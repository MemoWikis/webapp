using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TrueOrFalse.Tests
{
    [TestFixture]
    public class Serialize_and_init_solutionMetadata : BaseTest
    {
        [Test]
        public void Should_serialze_and_deserialize()
        {
            var solutionMeta = new SolutionMetadataText();
            solutionMeta.IsCaseSensitive = true; 

            var solutionToInit = new SolutionMetadataText();
            solutionToInit.Json = solutionMeta.Json;

            Assert.That(solutionMeta.IsCaseSensitive, Is.EqualTo(solutionToInit.IsCaseSensitive));
            Assert.That(solutionMeta.IsExactInput, Is.EqualTo(solutionToInit.IsExactInput));
            Assert.That(solutionMeta.Json, Is.EqualTo(solutionToInit.Json));
        }
    }
}
