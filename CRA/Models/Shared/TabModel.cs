using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA.Models.Shared
{
    public class TabModel : ClientBaseModel
    {
        public List<AccordionModel> Accordions { get; set; }
        public TabModel()
        {
            Accordions = new List<AccordionModel>();
        }
    }
}