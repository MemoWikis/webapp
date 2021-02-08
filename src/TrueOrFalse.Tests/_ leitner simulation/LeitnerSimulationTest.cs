using System; 
using System.IO;
using System.Text;
using NUnit.Framework;

[TestFixture]
public class LeitnerSimulationTest : BaseTest
{
    [Test]
    [Ignore("")]
    public void ExecuteSimulation()
    {
        for(var i = 1; i <= 5; i++) { 
            ExecuteSimulation(7, 10 * (i * 2));
        }

        for (var i = 1; i <= 7; i++){
            ExecuteSimulation(4 * i, 100);
        }
    }

    private static void ExecuteSimulation(int numberOfDays, int numberOfQuestions)
    {
        var leitnerSimulation = new LeitnerSimulation();
        leitnerSimulation.Start(numberOfDays, numberOfQuestions);

        var sb = new StringBuilder();

        sb.AppendLine($"Amount questions: {numberOfQuestions}");
        sb.AppendLine($"Amount days: {numberOfDays}");
        sb.AppendLine("");
        sb.AppendLine($"Total Leitner repititions: {leitnerSimulation.Days.GetSumOfRepetitions()}");
        sb.AppendLine("");

        sb.AppendLine("----------------------------------------------------------");
        sb.AppendLine("Leitner Details:");
        sb.AppendLine("");

        foreach (var day in leitnerSimulation.Days)
        {
            sb.AppendLine($"== Day == {day.Number}");
            foreach (var box in day.BoxesBefore)
            {
                sb.AppendLine(
                    $"Box{(box.ToRepeat ? "*" : "")}{box.Number}: " +
                    $"(before:) {box.Questions.Count} / (after:) {day.BoxesAfter.ByNumber(box.Number).Questions.Count}");
            }
            sb.AppendLine($"Sum of repititions: {day.GetSumOfRepetitions()}");
        }

        sb.AppendLine("----------------------------------------------------------");
        sb.AppendLine("Memucho Details:");
        sb.AppendLine("");

        var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"leitner-days-{numberOfDays}-questions-{numberOfQuestions}.txt");
        File.WriteAllText(fileName, sb.ToString());
    }

}

