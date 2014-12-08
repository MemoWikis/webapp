using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum ImageDeployability
{
    DeployabilityNotEvaluated = 0,
    ImageIsReadyToUse = 1,
    FurtherActionRequired = 2,
    ImageCurrentlyNotDeployable = 3,
    ImageRuledOutManually = 4
}
