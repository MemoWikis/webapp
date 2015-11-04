using System.Collections.Generic;

public interface IAnswerPattern
{
    string Name { get; }
    bool IsMatch(List<Answer> listOfAnswers);
}

