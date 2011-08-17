﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpTestsEx;
using TrueOrFalse.Core;

namespace TrueOrFalse.Tests
{
    [Category(TestCategories.UnitTest)]
    class Import_version_0_1_tests
    {
        private const string Xml = @"<trueorfalse>
                                        <version>0.1</version>
                                        <question>
                                            <text>Question 1</text>
                                            <answer>
                                                <text>Answer to question 1</text>
                                            </answer>
                                        </question>
                                        <question>
                                            <text>Question 2</text>
                                            <answer>
                                                <text>First answer to Question 2</text>
                                            </answer>
                                            <answer>
                                                <text>Second answer to Question 2</text>
                                            </answer>
                                        </question>
                                        <category>
                                            <name>Sport</name>
                                            <SubCategory>
                                                <name>Sportart</name>
                                                <type>OpenList</type>
                                                <item>
                                                    <name>Tennis</name>
                                                </item>
                                                <item>
                                                    <name>Fussball</name>
                                                </item>
                                            </SubCategory>
                                        </category>
                                    </trueorfalse>";


        [Test]
        public void Should_import_questions_and_answers()
        {
            var importer = new Importer(Xml);

            importer.Questions.Count().Should().Be.EqualTo(2);
            importer.Questions.First().Text.Should().Be.EqualTo("Question 1");
            importer.Questions.First().Answers.Single().Text.Should().Be.EqualTo("Answer to question 1");

            importer.Questions.Last().Text.Should().Be.EqualTo("Question 2");
            importer.Questions.Last().Answers.Count().Should().Be.EqualTo(2);
            importer.Questions.Last().Answers.First().Text.Should().Be.EqualTo("First answer to Question 2");
            importer.Questions.Last().Answers.Last().Text.Should().Be.EqualTo("Second answer to Question 2");
        }

        [Test]
        public void Should_import_categories()
        {
            var importer = new Importer(Xml);

            importer.Categories.Count().Should().Be.EqualTo(1);
            /// continue here
        }

    }
}
