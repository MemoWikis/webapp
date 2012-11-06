using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    public class AnswerHistory : IPersistable, WithDateCreated
    {
        public virtual int Id { get; set; }
        public virtual int UserId { get; set; }
        public virtual int QuestionId { get; set; }
        public virtual bool AnswerredCorrectly { get; set; }
        public virtual string AnswerText { get; set; }

        /// <summary>Duration</summary>
        public virtual int Milliseconds { get; set; }
        public virtual DateTime DateCreated { get; set; }
    }
}
