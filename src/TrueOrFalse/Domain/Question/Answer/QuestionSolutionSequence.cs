﻿
using System.Text.Json;

public class QuestionSolutionSequence : QuestionSolution
{
    public Dictionary<string, string> Rows;

    public override bool IsCorrect(string answer)
    {
        var values = JsonSerializer.Deserialize<string[]>(answer.Trim());
        return values.SequenceEqual(Rows.Values);
    }

    public override string CorrectAnswer()
    {
        return string.Join(", ", Rows.Values);
    }
}