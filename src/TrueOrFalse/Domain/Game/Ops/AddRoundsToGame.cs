using System.Linq;
using TrueOrFalse;

public class AddRoundsToGame : IRegisterAsInstancePerLifetime
{
    public void Run(Game game, bool multipleChoiceOnly = false)
    {
        game.Rounds.Clear();

        var allQuestions = game.Sets
            .SelectMany(x => x.QuestionsInSet)
            .GroupBy(x => x.Question.Id)
            .Select(x => x.First())
            .ToList();

        if (multipleChoiceOnly)
            allQuestions = allQuestions.Where(q => q.Question.SolutionType == SolutionType.MultipleChoice).ToList();

        allQuestions.Shuffle();

        for (var i = 0; i < game.RoundCount && i < allQuestions.Count; i++)
        {
            game.AddRound(new Round
            {
                Set = allQuestions[i].Set,
                Question = allQuestions[i].Question,
            });
        }

        if (game.RoundCount > allQuestions.Count)
            game.RoundCount = allQuestions.Count;
    }
}