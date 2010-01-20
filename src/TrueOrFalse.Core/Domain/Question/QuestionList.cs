﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core
{
    public class QuestionList : List<Question>
    {
        public Question GetById(int id)
        {
            return Find(question => question.Id == id);
        }
    }
}
