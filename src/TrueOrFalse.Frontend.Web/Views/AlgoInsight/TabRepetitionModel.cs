using System.Collections.Generic;

public class TabRepetitionModel : BaseModel
{
    public IList<AnswerPatternInfo> PatternInfos;

    public TabRepetitionModel()
    {
        PatternInfos = AnswerPatternInfoLoader.Run();
    }
}