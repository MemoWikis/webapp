using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    public class QuestionView : IPersistable, WithDateCreated
    {
        public virtual int Id { get; set; }

        public virtual int QuestionId { get; set; }
        public virtual int UserId { get; set; }

        public virtual DateTime DateCreated { get; set; }
    }
}
