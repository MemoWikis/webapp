using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpTestsEx;
using TrueOrFalse;

namespace TrueOrFalse.Tests
{
    [Category(TestCategories.UnitTest)]
    class Import_version_0_1_tests : BaseTest
    {
        private List<User> _users;

        [SetUp]
        public void SetUpBaseTest()
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
                                            <categories>
                                                <id>1</id>
                                            </categories>
                                        </question>
                                        <question>
                                            <text>Question 2</text>
                                            <creatorId>1</creatorId>
                                            <description>foo bar</description>
                                            <visibility>All</visibility>
                                            <solution>First answer to Question 2</solution>
                                            <categories>                                                
                                            </categories>
                                        </question>                                        
                                        <category>
                                            <id>1</id>
                                            <name>Sport</name>
                                            <relatedCategories>
                                            </relatedCategories>                                    
                                        </category>
                                        <category>
                                            <id>2</id>
                                            <name>Football</name>
                                            <relatedCategories>
                                                <id>1</id>
                                            </relatedCategories>
                                        </category>
                                    </trueorfalse>";


        [Test]
       
        public void Should_import_questions_and_answers()
        {
            var importer = Resolve<Importer>();
            var importerResutl = importer.Run(Xml);

            importerResutl.Questions.Count().Should().Be.EqualTo(2);
            importerResutl.Questions.First().Text.Should().Be.EqualTo("Question 1");
            importerResutl.Questions.First().Creator.Should().Be.EqualTo(_users.First());
            importerResutl.Questions.First().Solution.Should().Be.EqualTo("Answer to question 1");

            importerResutl.Questions.Last().Text.Should().Be.EqualTo("Question 2");
            importerResutl.Questions.Last().Creator.Should().Be.EqualTo(_users.First());
            importerResutl.Questions.Last().Solution.Should().Be.EqualTo("First answer to Question 2");
        }

        [Test]
        [Ignore("")]
        public void Should_import_categories()
        {
            var importer = Resolve<Importer>();
            var importerResult = importer.Run(Xml);

            importerResult.Categories.Count().Should().Be.EqualTo(2);
            importerResult.Categories.First().Name.Should().Be.EqualTo("Sport");
            importerResult.Categories.Last().Name.Should().Be.EqualTo("Football");
            importerResult.Categories.Last().ParentCategories().Single().Should().Be.SameInstanceAs(importerResult.Categories.First());
        }

    }
}
