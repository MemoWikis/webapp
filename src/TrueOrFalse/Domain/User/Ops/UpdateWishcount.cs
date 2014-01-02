using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse
{
    public class UpdateWishcount : IRegisterAsInstancePerLifetime
    {
        private readonly UserRepository _userRepository;
        private readonly GetWishQuestionCount _getWishQuestionCount;
        private readonly GetWishSetCount _getWishSetCount;

        public UpdateWishcount(
            UserRepository userRepository,
            GetWishQuestionCount getWishQuestionCount,
            GetWishSetCount getWishSetCount)
        {
            _userRepository = userRepository;
            _getWishQuestionCount = getWishQuestionCount;
            _getWishSetCount = getWishSetCount;
        }

        public void Run()
        {
            foreach (var user in _userRepository.GetAll())
                Run(user);
        }

        public void Run(User user)
        {
            user.WishCountQuestions = _getWishQuestionCount.Run(user.Id);
            user.WishCountSets = _getWishSetCount.Run(user.Id);
            _userRepository.Update(user);
        }
    }
}