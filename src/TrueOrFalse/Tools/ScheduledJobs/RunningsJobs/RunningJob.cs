﻿using System;
using Seedworks.Lib.Persistence;

public class RunningJob : Entity
{
    public virtual string Name { get; set; }
    public virtual DateTime StartAt { get; set; }
}