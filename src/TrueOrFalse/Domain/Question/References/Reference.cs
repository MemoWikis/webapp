using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    public class Reference : DomainEntity
    {
        public virtual Question Question { get; set; }
        public virtual Category Category { get; set; }

        public virtual string AdditionalInfo { get; set; }
        public virtual string FreeTextReference { get; set; }
    }
}