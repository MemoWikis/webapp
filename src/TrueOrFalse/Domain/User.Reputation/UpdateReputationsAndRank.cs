using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse
{
    public class UpdateReputationsAndRank : IRegisterAsInstancePerLifetime
    {
        private readonly UserRepository _userRepository;
        private readonly ReputationCalc _reputationCalc;

        public UpdateReputationsAndRank(
            UserRepository userRepository,
            ReputationCalc reputationCalc)
        {
            _userRepository = userRepository;
            _reputationCalc = reputationCalc;
        }

        public void Run()
        {
            var allUsers = _userRepository.GetAll();
            var results = allUsers
                .Select(user => _reputationCalc.Run(user))
                .OrderBy(r => r.TotalRepuation);

            var i = 0;
            foreach (var result in results)
            {
                i++;
                result.User.ReputationPos = i;
                result.User.Reputation = result.TotalRepuation;
                _userRepository.Update(result.User);
            }
        }
    }
}