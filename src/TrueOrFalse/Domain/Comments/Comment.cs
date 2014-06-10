﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seedworks.Lib.Persistence;
using TrueOrFalse;

[DebuggerDisplay("Id={Id} Text={Text}")]
public class Comment : DomainEntity
{
    public virtual CommentType Type  { get; set; }
    public virtual int TypeId { get; set; }

    public virtual Comment AnswerTo { get; set; }

    public virtual IList<Comment> Answers { get; set; }
    public virtual User Creator { get; set; }

    public virtual string Text { get; set; }

    public virtual bool ShouldRemove { get; set; }

    public virtual bool ShouldImprove { get; set; }

    public virtual string ShouldIds { get; set; }

    public Comment ()
    {
        Type = CommentType.AnswerQuestion;
    }
}