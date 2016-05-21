using Newtonsoft.Json;

public class TrainingPlanJson
{
    public string ToJson(TrainingPlan trainingPlan)
    {
        return JsonConvert.SerializeObject(trainingPlan);
    }

    public TrainingPlan Load(string trainngPlanJson, Date date)
    {
        var trainingPlan = JsonConvert.DeserializeObject<TrainingPlan>(trainngPlanJson);
        trainingPlan.Date = date;

        foreach (var trainingDate in trainingPlan.Dates)
        {
            trainingDate.TrainingPlan = trainingPlan;
        }

        return trainingPlan;
    }
}                                                                                                                                                                                                   