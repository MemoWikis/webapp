﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using TrueOrFalse.Core;

public class EditQuestionModel_to_Question : IRegisterAsInstancePerLifetime
{
    public Question Create(EditQuestionModel model, NameValueCollection postData)
    {
        var question = new Question();
        return Update(model, question, postData);
    }


    public Question Update(EditQuestionModel model, Question question, NameValueCollection postData)
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

        var serializer = new JavaScriptSerializer();
        switch (question.SolutionType)
        {
                case QuestionSolutionType.Exact:
                var solutionModel0 = new QuestionSoulutionExact();
                solutionModel0.FillFromPostData(postData);
                question.Solution = solutionModel0.Text;
                break;

            case QuestionSolutionType.Sequence:
                var solutionModel1 = new QuestionSolutionSequence();
                solutionModel1.FillFromPostData(postData);
                question.Solution = serializer.Serialize(solutionModel1);
                break;

            case QuestionSolutionType.MultipleChoice:
                var solutionModel2 = new QuestionSolutionMultipleChoice();
                solutionModel2.FillFromPostData(postData);
                question.Solution = serializer.Serialize(solutionModel2);
                break;
        }

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