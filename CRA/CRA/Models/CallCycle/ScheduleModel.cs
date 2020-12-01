using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA.Models.CallCycle
{
    public class ScheduleModel
    {
        public long UserId { get; set; }

        public List<ScheduleItemModel> Items { get; set; }

        public string Notes { get; set; }

        public ScheduleModel()
        {
            Items = new List<ScheduleItemModel>();
        }
    }
}