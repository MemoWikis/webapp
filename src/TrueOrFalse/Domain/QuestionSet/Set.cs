using System.Collections.Generic;
using System.Diagnostics;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    [DebuggerDisplay("Id={Id} Name={Name}")]
    public class Set : DomainEntity
    {
        public virtual string Name { get; set; }
        public virtual string Text { get; set; }

        public virtual IList<QuestionInSet> QuestionsInSet{ get; set;}
        public virtual User Creator { get; set; }

        public virtual IList<Category> Categories { get; set; }

        public virtual void Add(Question question){
            QuestionsInSet.Add(
                new QuestionInSet{
                        Set = this,
                        Question = question,
                        Sort = QuestionsInSet.Count + 1
                }
            );
        }

        public virtual void Add(IList<Question> questions){
            foreach (var question in questions){
                Add(question);
            }
        }

        public Set(){
            QuestionsInSet = new List<QuestionInSet>();
            Categories = new List<Category>();
        }
    }
}