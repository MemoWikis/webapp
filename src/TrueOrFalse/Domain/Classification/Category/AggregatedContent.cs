using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


[Serializable]
public class AggregatedContent
{
    public List<int> AggregatedSetsIds;
    
    [JsonIgnore]
    public IList<Set> AggregatedSets
    {
        get => AggregatedSetsIds == null
            ? new List<Set>()
            : Sl.SetRepo.GetByIds(AggregatedSetsIds);

        set { AggregatedSetsIds = value.Select(s => s.Id).ToList(); }
    }

    public List<int> AggregatedQuestionIds;

    [JsonIgnore]
    public IList<Question> AggregatedQuestions
    {
        get => AggregatedQuestionIds == null
            ? new List<Question>()
            : Sl.QuestionRepo.GetByIds(AggregatedQuestionIds);

        set { AggregatedQuestionIds = value.Select(q => q.Id).ToList(); }
    }

    [JsonIgnore]
    private readonly CategoryRepository _categoryRepository;

    public AggregatedContent()
    {
        _categoryRepository = Sl.R<CategoryRepository>();
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static AggregatedContent FromJson(string aggregatedContentJson)
    {
        return JsonConvert.DeserializeObject<AggregatedContent>(aggregatedContentJson ?? "") ?? new AggregatedContent();
    }

} 

