﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;

class Bus
{
    public static IBus  Get()
    {
        return RabbitHutch.CreateBus("host=localhost");
    }
}

