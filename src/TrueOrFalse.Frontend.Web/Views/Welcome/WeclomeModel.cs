using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueOrFalse.Core;

namespace TrueOrFalse.Frontend.Web.Models
{
    public class QuestionHomeModel : ModelBase
    {
        public QuestionList MostPopular;

        public QuestionHomeModel()
        {
            this.ShowLeftMenu = false;
        }
    }
}
