using System;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    [Serializable]
    public class Reference : DomainEntity
    {
        public virtual int Index { get; set; }
        public virtual Question Question { get; set; }
        public virtual Category Category { get; set; }
        public virtual ReferenceType ReferenceType { get; set; }
        public virtual string AdditionalInfo { get; set; }
        public virtual string ReferenceText { get; set; }
    }
}