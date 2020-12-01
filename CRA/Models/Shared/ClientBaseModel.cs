using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA.Models.Shared
{
    public class ClientBaseModel
    {
        public string ParentClientId { get; set; }
        public string ClientId { get; set; }
        public string Label { get; set; }
        public bool Active { get; set; }
        public string ClientCode { get; set; }
        public string ParentClientCode { get; set; }
        public string GetUniqueClientCode()
        {
            return $"{ParentClientCode}.{ClientCode}";
        }

        public string GetDataBoundUniqueClientCode()
        {
            return $"{ParentClientCode}.{ClientCode}".Replace("model.", "");
        }

        public string GetParentDataBoundUniqueClientCode()
        {
            return $"{ParentClientCode}".Replace($"{BindingContainer}.", "");
        }


        public string BindingContainer { get; set; }

        public ClientBaseModel()
        {
            BindingContainer = "model";
        }
    }
}