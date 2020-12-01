using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA.Models.Shared
{
    public class AccordionModel : ClientBaseModel
    {
    
        public List<SectionModel> Sections { get; set; }
        public AccordionModel()
        {
            Sections = new List<SectionModel>();
        }

    }
}