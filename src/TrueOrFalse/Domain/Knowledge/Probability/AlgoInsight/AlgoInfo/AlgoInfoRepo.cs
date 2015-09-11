using System.Collections.Generic;

public class AlgoInfoRepo
{
	public static List<AlgoInfo> GetAll()
	{
		return new List<AlgoInfo>
		{
            new AlgoInfo
            {
	            Id = 1,
                Name = "Simple 1",
                Algorithm = Sl.R<ProbabilityCalc_Simple1>()
            },
            new AlgoInfo
            {
	            Id = 2,
                Name = "Simple 2",
                Algorithm = Sl.R<ProbabilityCalc_Simple2>()
            },
            new AlgoInfo
            {
                Id = 3,
                Name = "Simple 3",
                Algorithm = Sl.R<ProbabilityCalc_Simple3>()
            }
        };
	}
}