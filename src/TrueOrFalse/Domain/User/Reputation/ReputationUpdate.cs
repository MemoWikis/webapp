public class ReputationUpdate : IRegisterAsInstancePerLifetime
{
    private readonly ReputationCalc _reputationCalc;
    private readonly UserRepo _userRepo;

    public ReputationUpdate(
        ReputationCalc reputationCalc,
        UserRepo userRepo)
    {
        _reputationCalc = reputationCalc;
        _userRepo = userRepo;
    }

    public void ForQuestion(int questionId)
    {
        Run(Sl.Resolve<QuestionRepo>().GetById(questionId).Creator);
    }

    public void ForSet(int setId)
    {
        Run(Sl.Resolve<SetRepo>().GetById(setId).Creator);
    }

    public void Run(User userToUpdate)
    {
        var oldReputation = userToUpdate.Reputation;
        var newReputation  = userToUpdate.Reputation = _reputationCalc.Run(userToUpdate).TotalRepuation;

        var users = _userRepo.GetWhereReputationIsBetween(newReputation, oldReputation);
        for (int i = 0; i < users.Count; i++)
        {
            userToUpdate.ReputationPos = users[i].ReputationPos;
            if (newReputation < oldReputation)
                users[i].ReputationPos--;
            else
                users[i].ReputationPos++;

            _userRepo.Update(users[i]);
        }

        _userRepo.Update(userToUpdate);
    }
}