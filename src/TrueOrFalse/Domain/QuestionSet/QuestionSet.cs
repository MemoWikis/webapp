using System.Collections.Generic;
using System.Diagnostics;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    [DebuggerDisplay("Id={Id} Name={Name}")]
    public class QuestionSet : DomainEntity
    {
        public virtual string Name { get; set; }
        public virtual string Text { get; set; }

        public virtual IList<QuestionInSet> QuestionsInSet{ get; set;}
        public virtual User Creator { get; set; }

        public virtual void Add(Question question){
            QuestionsInSet.Add(
                new QuestionInSet{
                        Set = this,
                        Question = question,
                        Index = QuestionsInSet.Count + 1
                }
            );
        }

        public virtual void Add(IList<Question> questions){
            foreach (var question in questions){
                Add(question);
            }
        }

        public virtual void Add(IList<QuestionInSet> questionsInSet){
            foreach (var questionInSet in questionsInSet){
                questionInSet.Index = QuestionsInSet.Count + 1;
                QuestionsInSet.Add(questionInSet);
            }
        }

        public QuestionSet(){
            QuestionsInSet = new List<QuestionInSet>();
        }
    }
}