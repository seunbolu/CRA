using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA.Models.Shared
{
    public class PostDataModel
    {

        public PostDataModel()
        {
            ControlData = new List<ControlDataModel>();
            TableData = new List<TableDataModel>();
        }
        public List<ControlDataModel> ControlData { get; set; }
        public List<TableDataModel> TableData { get; set; }

    }
}