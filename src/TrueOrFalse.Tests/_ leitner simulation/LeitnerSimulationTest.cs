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

        var memuchoTrainingPlan = Get_comparison_with_training_plan_creator(numberOfQuestions, numberOfDays);

        var sb = new StringBuilder();

        sb.AppendLine($"Amount questions: {numberOfQuestions}");
        sb.AppendLine($"Amount days: {numberOfDays}");
        sb.AppendLine("");
        sb.AppendLine($"Total Leitner repititions: {leitnerSimulation.Days.GetSumOfRepetitions()}");
        sb.AppendLine("");

        TrainingPlan.AvgRepetitionsPerTraingDate = 2;
        sb.AppendLine("Total memucho repetitions: " + memuchoTrainingPlan.GetSumOfRepetitions() + " (avg. 2 repetitions per training session)");
        TrainingPlan.AvgRepetitionsPerTraingDate = 3;
        sb.AppendLine("Total memucho repetitions: " + memuchoTrainingPlan.GetSumOfRepetitions() + " (avg. 3 repetitions per training session)");


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
        TrainingPlan.AvgRepetitionsPerTraingDate = 2;
        sb.AppendLine($"(avg. {TrainingPlan.AvgRepetitionsPerTraingDate} repetitions per training session)");
        sb.AppendLine("");

        foreach (var trainingDate in memuchoTrainingPlan.Dates)
            sb.AppendLine($"Day: {(trainingDate.DateTime - DateTime.Now).Days} " +
                          $"{trainingDate.DateTime} repetitions: {trainingDate.AllQuestionsInTraining.Count * TrainingPlan.AvgRepetitionsPerTraingDate} ");
        
        var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"leitner-days-{numberOfDays}-questions-{numberOfQuestions}.txt");
        File.WriteAllText(fileName, sb.ToString());
    }

    public static TrainingPlan Get_comparison_with_training_plan_creator(int numberOfQuestions, int numberOfDays)
    {
        var date = new Date
        {
            DateTime = DateTime.Now.AddDays(numberOfDays),
            User = ContextUser.GetUser()
        };

        date.TrainingPlan = TrainingPlanCreator.Run(date, new TrainingPlanSettings
        {
            QuestionsPerDate_IdealAmount = 20,
            MinSpacingBetweenSessionsInMinutes = 1200,
        });

        return date.TrainingPlan;
    }
}

