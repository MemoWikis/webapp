using System.Linq;

namespace Tool.Muse
{
    public class OscMessage
    {
        public string Path;

        public string Data
        {
            get { return DataRaw.Select(x => x.ToString()).Aggregate((a, b) => a + b); }
        }

        public string Full()
        {
            return Path + " " + Data; 
        }

        public object[] DataRaw;

        public bool IsConcentrationValue { get { return Path.StartsWith("/muse/elements/experimental/concentration"); } }
        public bool IsConcentrationMellow { get { return Path.StartsWith("/muse/elements/experimental/mellow"); } }
        public bool IsOnHead { get { return Path.StartsWith("/muse/elements/touching_forehead"); } }
        public bool IsHorseHoe { get { return Path.StartsWith("/muse/elements/horseshoe"); } }
        public bool IsQuality { get { return Path.StartsWith("/muse/elements/is_good"); } }
        public bool IsBattery { get { return Path.StartsWith("/muse/batt"); } }
    }
}