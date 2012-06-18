using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using TrueOrFalse.Core;

public class EditQuestionModel_to_Question : IRegisterAsInstancePerLifetime
{
    public Question Create(EditQuestionModel model)
    {
        var question = new Question();
        return Update(model, question);
    }


    public Question Update(EditQuestionModel model, Question question)
    {
        question.Text = model.Question;
        question.Description = model.Description;
        question.Categories.Clear();

        foreach (var category in model.Categories)
        {
            AddCategory(question, category);
        }

        question.Solution = model.Solution;
        question.SolutionType = (QuestionSolutionType) Enum.Parse(typeof(QuestionSolutionType), model.SolutionType);
        return question;
    }

    private void AddCategory(Question question, string categoryName)
    {
        var category = ServiceLocator.Resolve<CategoryRepository>().GetByName(categoryName);

        if (category == null)
            throw new InvalidDataException();

        question.Categories.Add(category);
    }


}