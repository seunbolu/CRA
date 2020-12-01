using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA.Models.Shared
{
    public class SectionModel : ClientBaseModel
    {
     
        public List<AccordionModel> Accordions { get; set; }

        public List<ItemModel> Items { get; set; }

        public bool SaveButtonVisible { get; set; }

        public string Context { get; set; }
        //public long ContextId { get; set; }

        public bool Deleted { get; set; }
        public string ClientCallBack { get; set; }

        public SectionModel()
        {
            Accordions = new List<AccordionModel>();
            Items = new List<ItemModel>();
            SaveButtonVisible = true;
        }

    }
}