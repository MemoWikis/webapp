using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using TrueOrFalse.Core;

namespace TrueOrFalse.View.Web.Views.Questions.Edit
{
    public class EditQuestionModel_to_Question
    {
        public void Create(EditQuestionModel model)
        {
            var question = new Question();
            question.Answers.Add(new Answer());
            return Update(model, question);            
        }


        public Question Update(EditQuestionModel model, Question question)
        {
            question.Text = model.Question;
            question.Description = model.Description;
            question.Categories.Clear();

            AddCategory(question, model.Category1);
            AddCategory(question, model.Category2);
            AddCategory(question, model.Category3);
            AddCategory(question, model.Category4);
            AddCategory(question, model.Category5);

            question.Answers[0].Text = model.Answer;
            return question;
        }

        private void AddCategory(Question question, string categoryName)
        {
            if (String.IsNullOrEmpty(categoryName))
                return;

            var category = ServiceLocator.Resolve<CategoryRepository>().GetByName(categoryName);

            if (category == null)
                throw new InvalidDataException();

            question.Categories.Add(category);
        }


    }
}