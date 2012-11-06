using NHibernate;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    public class QuestionRepository : RepositoryDb<Question> 
    {
        public QuestionRepository(ISession session) : base(session)
        {
        }

        public override void Update(Question question)
        {
            base.Update(question);
            Flush();            
        }

        public override void Create(Question question)
        {
            base.Create(question);
            Flush();
        }
    }
}