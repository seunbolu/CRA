using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA.Models.Shared
{
    public class ItemModel : ClientBaseModel
    {

        public string ItemType { get; set; }

        public string Name { get; set; }
        public long Index { get; set; }
        public string DependsOnCode { get; set; }
        public string DependsOnAssertValue { get; set; }
        public string[] LabelList { get; set; }
        public string[] ValueList { get; set; }

        public string[] CodeList { get; set; }

        public string[] TypeList { get; set; }

        public decimal Step { get; set; }

        public bool ComparePrimitive { get; set; }

        public string DefaultDropDownValue { get; set; }
        public bool Disabled { get; set; }

        public bool IsCalculated { get; set; }

        public bool IsIVFField { get; set; }


        public bool KeyClinicalIndicator { get; set; }

        public string KeyClinicalIndicatorAssertValue { get; set; }

        public bool Required { get; set; }

        public bool AcuityICU { get; set; }
        public bool AcuityINT { get; set; }
        public bool AcuityMed { get; set; }
        public string AcuityAsserValue { get; set; }


        public bool SNF { get; set; }

        public bool SSSN { get; set; }

        public bool LTACH { get; set; }

        public ItemModel()
        {
            ItemType = "Text";
            LTACH = true;
            Required = true;
        }
    }
}