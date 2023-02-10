using System.Collections.Generic;

public class AlgoInfoRepo
{
	public static List<AlgoInfo> GetAll()
	{
		return new List<AlgoInfo>
		{
            new()
            {
	            Id = 1,
                Name = "Simple 1",
                Details = "Wenn keine Historie, dann Wahrscheinlichkeit der Frage verwenden.",
                Algorithm = Sl.R<ProbabilityCalc_Simple1>()
            },
            new()
            {
	            Id = 2,
                Name = "Simple 2",
                Details = "Wenn keine Historie, dann Wahrscheinlichkeit des Nutzers verwenden.",
                Algorithm = Sl.R<ProbabilityCalc_Simple2>()
            },
            new()
            {
                Id = 3,
                Name = "Simple 3",
                Details = "Wenn keine Historie, dann Wahrscheinlichkeit des Themas verwenden.",
                Algorithm = Sl.R<ProbabilityCalc_Simple3>()
            }
        };
	}
}