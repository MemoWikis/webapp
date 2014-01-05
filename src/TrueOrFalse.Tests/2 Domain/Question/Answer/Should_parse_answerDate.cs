using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TrueOrFalse.Tests
{
    public class Should_parse_answerDate : BaseTest
    {
        [Test]
        public void Run()
        {
            Assert.That(DateAnswerParser.Run("22.12.2014").IsValid, Is.True);
            Assert.That(DateAnswerParser.Run("22.12.2014").Day, Is.EqualTo(22));
            Assert.That(DateAnswerParser.Run("22.12.2014").Month, Is.EqualTo(12));
            Assert.That(DateAnswerParser.Run("22.12.2014").Year, Is.EqualTo(2014));
            Assert.That(DateAnswerParser.Run("22.12.2014").Precision, Is.EqualTo(DatePrecision.Day));

            Assert.That(DateAnswerParser.Run("12.2014").IsValid, Is.True);
            Assert.That(DateAnswerParser.Run("12.2014").Year, Is.EqualTo(2014));
            Assert.That(DateAnswerParser.Run("12.2014").Month, Is.EqualTo(12));
            Assert.That(DateAnswerParser.Run("12.2014").Precision, Is.EqualTo(DatePrecision.Month));

            Assert.That(DateAnswerParser.Run("3 Jh").IsValid, Is.True);
            Assert.That(DateAnswerParser.Run("3 Jh").Year, Is.EqualTo(3));
            Assert.That(DateAnswerParser.Run("3 Jh").Precision, Is.EqualTo(DatePrecision.Century));
            Assert.That(DateAnswerParser.Run("3 Jh").IsValid, Is.True);
            Assert.That(DateAnswerParser.Run("3 : Jh").IsValid, Is.False);
            Assert.That(DateAnswerParser.Run("31 10 Jh").IsValid, Is.False);
            Assert.That(DateAnswerParser.Run("Jh").IsValid, Is.False);

            Assert.That(DateAnswerParser.Run("5 Jt").IsValid, Is.True);
            Assert.That(DateAnswerParser.Run("5 Jt").Year, Is.EqualTo(5));
            Assert.That(DateAnswerParser.Run("5 Jt").Precision, Is.EqualTo(DatePrecision.Millenium));

            Assert.That(DateAnswerParser.Run("1999").IsValid, Is.True);
            Assert.That(DateAnswerParser.Run("1999").Year, Is.EqualTo(1999));
            Assert.That(DateAnswerParser.Run("1999").Precision, Is.EqualTo(DatePrecision.Year));
        }
    }
}
