using System.Collections.Generic;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Models;

namespace TrueOrFalse.View.Web.Views.Summary
{
    public class SummaryModel : ModelBase
    {

        public IEnumerable<SelectListItem> KenDevelopmentTypes
        {
            get
            {
                return new List<SelectListItem>
                           {
                               new SelectListItem {Text = "Fragen"},
                               new SelectListItem {Text = "Kurse"}
                           };
            }
        }

        public IEnumerable<SelectListItem> KenDevelopmentPeriod
        {
            get
            {
                return new List<SelectListItem>
                           {
                               new SelectListItem {Text = "4 Wochen"},
                               new SelectListItem {Text = "6 Monate"}
                           };
            }
        }

        public SummaryModel()
        {
            ShowLeftMenu_Nav();
        }
    }
}