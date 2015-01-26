using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse
{
    public class ReputationUpdate : IRegisterAsInstancePerLifetime
    {
        private readonly ReputationCalc _reputationCalc;
        private readonly UserRepository _userRepository;

        public ReputationUpdate(
            ReputationCalc reputationCalc,
            UserRepository userRepository)
        {
            _reputationCalc = reputationCalc;
            _userRepository = userRepository;
        }

        public void ForQuestion(int questionId)
        {
            Run(Sl.Resolve<QuestionRepository>().GetById(questionId).Creator);
        }

        public void ForSet(int setId)
        {
            Run(Sl.Resolve<SetRepository>().GetById(setId).Creator);
        }

        public void Run(User userToUpdate)
        {
            var oldReputation = userToUpdate.Reputation;
            var newReputation  = userToUpdate.Reputation = _reputationCalc.Run(userToUpdate).TotalRepuation;

            var users = _userRepository.GetWhereReputationIsBetween(newReputation, oldReputation);
            for (int i = 0; i < users.Count; i++)
            {
                userToUpdate.ReputationPos = users[i].ReputationPos;
                if (newReputation < oldReputation)
                    users[i].ReputationPos--;
                else
                    users[i].ReputationPos++;

                _userRepository.Update(users[i]);
            }

            _userRepository.Update(userToUpdate);
        }
    }
}
