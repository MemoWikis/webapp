using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpTestsEx;
using TrueOrFalse.Core;

namespace TrueOrFalse.Tests
{
    [Category(TestCategories.UnitTest)]
    class Import_version_0_1_tests : BaseTest
    {
        private List<User> _users;

        [SetUp]
        public void SetUp()
        {
           _users =  Resolve<SampleData>().CreateUsers();
        }

        private const string Xml = @"<trueorfalse>
                                        <version>0.1</version>
                                        <question>
                                            <text>Question 1</text>
                                            <creatorId>1</creatorId>
                                            <description>foo bar</description>
                                            <visibility>All</visibility>
                                            <solution>Answer to question 1</solution>
                                        </question>
                                        <question>
                                            <text>Question 2</text>
                                            <creatorId>1</creatorId>
                                            <description>foo bar</description>
                                            <visibility>All</visibility>
                                            <solution>First answer to Question 2</solution>
                                        </question>
                                        <category>
                                            <name>Sport</name>
                                            <subCategory>
                                                <name>Sportart</name>
                                                <type>OpenList</type>
                                                <item>
                                                    <name>Tennis</name>
                                                </item>
                                                <item>
                                                    <name>Fussball</name>
                                                </item>
                                            </subCategory>
                                        </category>
                                    </trueorfalse>";


        [Test]
        public void Should_import_questions_and_answers()
        {
            var importer = Resolve<Importer>();
            importer.Run(Xml);

            importer.Questions.Count().Should().Be.EqualTo(2);
            importer.Questions.First().Text.Should().Be.EqualTo("Question 1");
            importer.Questions.First().Creator.Should().Be.EqualTo(_users.First());
            importer.Questions.First().Solution.Should().Be.EqualTo("Answer to question 1");

            importer.Questions.Last().Text.Should().Be.EqualTo("Question 2");
            importer.Questions.Last().Creator.Should().Be.EqualTo(_users.First());
            importer.Questions.Last().Solution.Should().Be.EqualTo("First answer to Question 2");
        }

        [Test]
        public void Should_import_categories()
        {
            var importer = Resolve<Importer>();
            importer.Run(Xml);

            importer.Categories.Count().Should().Be.EqualTo(1);
            importer.Categories.Single().Name.Should().Be.EqualTo("Sport");
            importer.Categories.Single().SubCategories.Count.Should().Be.EqualTo(1);
            importer.Categories.Single().SubCategories.Single().Name.Should().Be.EqualTo("Sportart");
            importer.Categories.Single().SubCategories.Single().Type.Should().Be.EqualTo(SubCategoryType.OpenList);
            importer.Categories.Single().SubCategories.Single().Items.Count.Should().Be.EqualTo(2);
            importer.Categories.Single().SubCategories.Single().Items.First().Name.Should().Be.EqualTo("Tennis");
            importer.Categories.Single().SubCategories.Single().Items.Last().Name.Should().Be.EqualTo("Fussball");
        }

    }
}
