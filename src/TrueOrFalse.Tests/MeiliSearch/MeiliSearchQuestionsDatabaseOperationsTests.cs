using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using TrueOrFalse.Search;

namespace TrueOrFalse.Tests.MeiliSearch;

internal class MeiliSearchQuestionsDatabaseOperationsTests : MeiliSearchBase
{
    [Test(Description = "Set TestQuestion in MeiliSearch")]
    public async Task CreateQuestionTest()
    {
        //construction
        await DeleteQuestions();
        var question = new Question
        {
            Id = 12,
            Creator = new User
            {
                Id = 15
            },
            Description = "Description",
            Categories = new List<Category>{ new(){ Name = "Daniel", Id = 15}},
            Solution = "Solution",
            SolutionType = SolutionType.Date,
            Text = "Text"
        };

        //Execution
        var taskId = 
            (await MeiliSearchQuestionsDatabaseOperations
                .CreateAsync(question, QuestionsTest)
                .ConfigureAwait(false))
            .TaskUid;
        await client.WaitForTaskAsync(taskId);

        var index = client.Index(QuestionsTest);
        var result = 
            (await index.SearchAsync<MeiliSearchQuestionMap>(question.Text)
                .ConfigureAwait(false))
            .Hits
            .ToList();
        var questionMap = result.First();

        //Tests 
        Assert.AreEqual(result.GetType(), typeof(List<MeiliSearchQuestionMap>));
        Assert.True(result.Count == 1);

        Assert.AreEqual(questionMap.Text, question.Text);
        Assert.AreEqual(15, questionMap.CategoryIds.Single());
        Assert.AreEqual(questionMap.Id, question.Id);
        Assert.AreEqual(questionMap.Description, question.Description);
        Assert.AreEqual(questionMap.Solution, question.Solution);
        Assert.AreEqual(questionMap.SolutionType, (int)question.SolutionType);
        Assert.AreEqual(questionMap.Categories.Single(), question.Categories.Single().Name);
        Assert.AreEqual(questionMap.CreatorId, question.Creator.Id);
    }

    [Test(Description = "Update TestQuestion in MeiliSearch")]
    public async Task UpdateQuestionTest()
    {
        //construction
        await DeleteQuestions();
        var question = new Question
        {
            Id = 12,
            Creator = new User
            {
                Id = 15
            },
            Description = "Description",
            Categories = new List<Category> { new() { Name = "Daniel", Id = 15 } },
            Solution = "Solution",
            SolutionType = SolutionType.Date,
            Text = "Text"
        };

        //Execution
        var taskId =
            (await MeiliSearchQuestionsDatabaseOperations
                .CreateAsync(question, QuestionsTest)
                .ConfigureAwait(false))
            .TaskUid;
        await client.WaitForTaskAsync(taskId);

        question.Text = "Ratte";
        taskId = 
            (await MeiliSearchQuestionsDatabaseOperations
                .UpdateAsync(question, QuestionsTest)
                .ConfigureAwait(false))
            .TaskUid;
        await client.WaitForTaskAsync(taskId);

        var index = client.Index(QuestionsTest);
        var result =
            (await index.SearchAsync<MeiliSearchQuestionMap>(question.Text)
                .ConfigureAwait(false))
            .Hits
            .ToList();
        var questionMap = result.First();

        //Tests 
        Assert.AreEqual(result.GetType(), typeof(List<MeiliSearchQuestionMap>));
        Assert.True(result.Count == 1);

        Assert.AreEqual(questionMap.Text, question.Text);
        Assert.AreEqual(15, questionMap.CategoryIds.Single());
        Assert.AreEqual(questionMap.Id, question.Id);
        Assert.AreEqual(questionMap.Description, question.Description);
        Assert.AreEqual(questionMap.Solution, question.Solution);
        Assert.AreEqual(questionMap.SolutionType, (int)question.SolutionType);
        Assert.AreEqual(questionMap.Categories.Single(), question.Categories.Single().Name);
        Assert.AreEqual(questionMap.CreatorId, question.Creator.Id);
    }

    [Test(Description = "Delete TestQuestion in MeiliSearch")]
    public async Task DeleteQuestionTest()
    {
        //construction
        await DeleteQuestions();
        var question = new Question
        {
            Id = 12,
            Creator = new User
            {
                Id = 15
            },
            Description = "Description",
            Categories = new List<Category> { new() { Name = "Daniel", Id = 15 } },
            Solution = "Solution",
            SolutionType = SolutionType.Date,
            Text = "Text"
        };

        //Execution
        var taskId =
            (await MeiliSearchQuestionsDatabaseOperations
                .CreateAsync(question, QuestionsTest)
                .ConfigureAwait(false))
            .TaskUid;
        await client.WaitForTaskAsync(taskId);

       
        taskId =
            (await MeiliSearchQuestionsDatabaseOperations
                .DeleteAsync(question, QuestionsTest)
                .ConfigureAwait(false))
            .TaskUid;
        await client.WaitForTaskAsync(taskId);

        var index = client.Index(QuestionsTest);
        var result =
            (await index.SearchAsync<MeiliSearchQuestionMap>(question.Text)
                .ConfigureAwait(false))
            .Hits
            .ToList();
        var questionMap = result.FirstOrDefault();

        //Tests 
        Assert.AreEqual(result.GetType(), typeof(List<MeiliSearchQuestionMap>));
        Assert.IsNull(questionMap);


    }
}