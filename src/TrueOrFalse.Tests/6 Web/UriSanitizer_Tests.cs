﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TrueOrFalse.Web
{
    [TestFixture]
    public class UriSanitizer_Tests
    {
        [Test]
        public void Should_remove_and_replace_invalid_characters()
        {
            Assert.That(UriSanitizer.Run("Question!"), Is.EqualTo("Question"));
            Assert.That(UriSanitizer.Run("What?-_.,"), Is.EqualTo("What-_"));
            Assert.That(UriSanitizer.Run("What why who?"), Is.EqualTo("What_why_who"));
            Assert.That(UriSanitizer.Run("Ä-Ö-Ü?"), Is.EqualTo("Ae-Oe-Ue"));
        }
    }
}
