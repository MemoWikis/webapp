using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse
{
    public enum ManualImageEvaluation
    {
        ImageNotEvaluated = 0,
        ImageCheckedForCustomAttributionAndAuthorized = 1,
        NotAllRequirementsMetYet = 2,
        ImageManuallyRuledOut = 3
    }
}
